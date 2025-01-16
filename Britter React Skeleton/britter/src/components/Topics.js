import React, { useState, useEffect, useRef } from "react";
import "../styles.css";
import Post from "./Post";

const Topics = () => {
    const [topics, setTopics] = useState([]);
    const [selectedTopic, setSelectedTopic] = useState(null);
    const [newTopic, setNewTopic] = useState({ title: "", description: "" });
    const [editTopic, setEditTopic] = useState(null);
    const [error, setError] = useState("");
    const [showDialog, setShowDialog] = useState(false);
    const [showEditDialog, setShowEditDialog] = useState(false);

    const dialogRef = useRef(null);
    const editDialogRef = useRef(null);

    const getTopics = async () => {
        const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/Topic`);
        if (response.ok) {
            const data = await response.json();
            setTopics(data);
        } else {
            console.error('Error fetching topics:', response);
        }
    };

    useEffect(() => {
        getTopics();
    }, []);

    const handleCreateTopic = async () => {
        if (!newTopic.title.trim()) {
            setError("Title cannot be empty");
            return;
        }
        try {
            const url = `${process.env.REACT_APP_API_ADDRESS}/Topic/Create/`;
            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                credentials: 'include',
                body: JSON.stringify(newTopic)
            });

            if (response.ok) {
                getTopics();
                setShowDialog(false);
                setNewTopic({ title: "", description: "" });
            } else {
                const errorData = await response.json();
                setError(errorData.message || "Login failed"); // errorData.message is the message from the server - Needs to be added to API
            }
        } catch (error) {
            console.error('There has been a problem with your fetch operation:', error);
        }
    };

    const handleEditTopic = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/Topic/Edit?id=${editTopic.id}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
                body: JSON.stringify(editTopic)
            });

            if (response.ok) {
                getTopics();
                setEditTopic(null);
                setShowEditDialog(false);
            } else {
                console.error('Error editing topic:', response);
            }
        } catch (error) {
            console.error('Error editing topic:', error);
        }
    };

    const handleDeleteTopic = async (id) => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/Topic/Delete?id=${id}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'text/plain;charset=UTF-8'
                },
                credentials: 'include',
                body: JSON.stringify({ id })
            });

            if (response.ok) {
                setTopics(topics.filter(topic => topic.id !== id));
                if (selectedTopic && selectedTopic.id === id) {
                    setSelectedTopic(null);
                }
            } else {
                console.error('Error deleting topic:', response);
            }
        } catch (error) {
            console.error('Error deleting topic:', error);
        }
    };

    const handleClickOutside = (event) => {
        if (dialogRef.current && !dialogRef.current.contains(event.target)) {
            setShowDialog(false);
        }
        if (editDialogRef.current && !editDialogRef.current.contains(event.target)) {
            setShowEditDialog(false);
        }
    };

    useEffect(() => {
        if (showDialog || showEditDialog) {
            document.addEventListener("mousedown", handleClickOutside);
        } else {
            document.removeEventListener("mousedown", handleClickOutside);
        }
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [showDialog, showEditDialog]);

    const handleTopicClick = (topic) => {
        setSelectedTopic(selectedTopic && selectedTopic.id === topic.id ? null : topic);
    };

    return (
        <div className="topics-container">
            <div className="sidebar">
                <button className="create-button" onClick={() => setShowDialog(true)}>Create Topic</button>
                <ul className="topics-list">
                    {topics.map(topic => (
                        <li key={topic.id} className="topic-item">
                            <div className="topic-header" onClick={() => handleTopicClick(topic)}>
                                {topic.title}
                                {selectedTopic && selectedTopic.id === topic.id && (
                                    <div className="dropdown">
                                        <button className="edit-button" onClick={() => { setEditTopic(topic); setShowEditDialog(true); }}>Edit</button>
                                        <button className="delete-button" onClick={() => handleDeleteTopic(topic.id)}>Delete</button>
                                    </div>
                                )}
                            </div>
                        </li>
                    ))}
                </ul>
            </div>
            <div className="content">
                {selectedTopic ? (
                    <div className="posts-container">
                        <h2 className="post-title">{selectedTopic.title}</h2>
                        <p className="post-description">{selectedTopic.description}</p>
                        <Post topicId={selectedTopic.id} />
                    </div>
                ) : (
                    <p>Select a topic to view posts</p>
                )}
            </div>
            {showDialog && (
                <div className="dialog-backdrop">
                    <div className="dialog" ref={dialogRef}>
                        <h2>Create Topic</h2>
                        {error && <p className="error">{error}</p>}
                        <input
                            type="text"
                            placeholder="Title"
                            value={newTopic.title}
                            onChange={(e) => setNewTopic({ ...newTopic, title: e.target.value })}
                        />
                        <input
                            type="text"
                            placeholder="Description"
                            value={newTopic.description}
                            onChange={(e) => setNewTopic({ ...newTopic, description: e.target.value })}
                        />
                        <button className="save-button" onClick={handleCreateTopic}>Save</button>
                        <button className="cancel-button" onClick={() => setShowDialog(false)}>Cancel</button>
                    </div>
                </div>
            )}
            {showEditDialog && (
                <div className="dialog-backdrop">
                    <div className="dialog" ref={editDialogRef}>
                        <h2>Edit Topic</h2>
                        {error && <p className="error">{error}</p>}
                        <input
                            type="text"
                            placeholder="Title"
                            value={editTopic.title}
                            onChange={(e) => setEditTopic({ ...editTopic, title: e.target.value })}
                        />
                        <input
                            type="text"
                            placeholder="Description"
                            value={editTopic.description}
                            onChange={(e) => setEditTopic({ ...editTopic, description: e.target.value })}
                        />
                        <button className="save-button" onClick={handleEditTopic}>Save</button>
                        <button className="cancel-button" onClick={() => setShowEditDialog(false)}>Cancel</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Topics;