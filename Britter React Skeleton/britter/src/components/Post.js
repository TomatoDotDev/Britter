import React, { useState, useEffect, useRef } from "react";
import "../styles.css";
import upvoteIcon from "../Icons/UpvoteIcon.svg";
import downvoteIcon from "../Icons/DownvoteIcon.svg";

const Post = ({ topicId }) => {
    const [posts, setPosts] = useState([]);
    const [currentUser, setCurrentUser] = useState(null);
    const [editPostContent, setEditPostContent] = useState("");
    const [editPostId, setEditPostId] = useState(null);
    const [showEditDialog, setShowEditDialog] = useState(false);
    const [selectedCommentId, setSelectedCommentId] = useState(null);
    const [commentContents, setCommentContents] = useState({});
    const editDialogRef = useRef(null);

    useEffect(() => {
        if (topicId) {
            getPosts();
        }
        getUser();
    }, [topicId]);

    const getUser = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/manage/info`, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                },
                credentials: 'include'
            });
            if (response.ok) {
                const data = await response.json();
                setCurrentUser(data.email);
            } else {
                console.error('Error fetching user:', response);
            }
        } catch (error) {
            console.error('Error fetching user:', error);
        }
    };

    const getPosts = async () => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/Post`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                credentials: 'include',
                body: JSON.stringify({ topicId })
            });
            if (response.ok) {
                const data = await response.json();
                setPosts(data.map(post => ({ ...post, upvoted: false, downvoted: false, voted: false })));
            } else {
                console.error('Error fetching posts:', response);
            }
        } catch (error) {
            console.error('Error fetching posts:', error);
        }
    };

    const createPost = async (content, parentPostId = null) => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/Post/Create`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
                body: JSON.stringify({
                    parentPostId: parentPostId,
                    content: content,
                    topicId: topicId
                })
            });

            if (response.ok) {
                getPosts();
            } else {
                console.error("Error creating post:", response.statusText);
            }
        } catch (error) {
            console.error("Error creating post:", error);
        }
    };

    const editPost = async (postId, content) => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/Post/Edit?id=${postId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
                body: content 
            });
            if (response.ok) {
                getPosts();
            } else {
                console.error("Error editing post:", response.statusText);
            }
        } catch (error) {
            console.error("Error editing post:", error);
        }
    };

    const deletePost = async (postId) => {
        try {
            await fetch(`${process.env.REACT_APP_API_ADDRESS}/Post/Delete`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'text/plain;charset=UTF-8'
                },
                credentials: 'include',
                body: JSON.stringify({ postId })
            });
            setPosts(posts.filter(post => post.id !== postId));
        } catch (error) {
            console.error("Error deleting post:", error);
        }
    };

    const handleUpvote = async (postId) => {
        const post = posts.find(post => post.id === postId);
        if (!post.upvoted && !post.voted) {
            try {
                const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/Vote/Create`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    credentials: 'include',
                    body: JSON.stringify({
                        postId: postId,
                        voteType: 1
                    })
                });

                if (response.ok) {
                    setPosts(posts.map(post => {
                        if (post.id === postId) {
                            if (post.downvoted) {
                                return { ...post, upvotes: post.upvotes + 1, downvotes: post.downvotes - 1, upvoted: true, downvoted: false, voted: true };
                            } else {
                                return { ...post, upvotes: post.upvotes + 1, upvoted: true, voted: true };
                            }
                        }
                        return post;
                    }));
                } else {
                    console.error("Error upvoting post:", response.statusText);
                }
            } catch (error) {
                console.error("Error upvoting post:", error);
            }
        }
    };

    const handleDownvote = async (postId) => {
        const post = posts.find(post => post.id === postId);
        if (!post.downvoted && !post.voted) {
            try {
                const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/Vote/Create`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    credentials: 'include',
                    body: JSON.stringify({
                        postId: postId,
                        voteType: -1
                    })
                });

                if (response.ok) {
                    setPosts(posts.map(post => {
                        if (post.id === postId) {
                            if (post.upvoted) {
                                return { ...post, downvotes: post.downvotes + 1, upvotes: post.upvotes - 1, downvoted: true, upvoted: false, voted: true };
                            } else {
                                return { ...post, downvotes: post.downvotes + 1, downvoted: true, voted: true };
                            }
                        }
                        return post;
                    }));
                } else {
                    console.error("Error downvoting post:", response.statusText);
                }
            } catch (error) {
                console.error("Error downvoting post:", error);
            }
        }
    };

    const handleEditClick = (post) => {
        setEditPostId(post.id);
        setEditPostContent(post.content);
        setShowEditDialog(true);
    };

    const handleEditConfirm = () => {
        editPost(editPostId, editPostContent);
        setShowEditDialog(false);
    };

    const handleClickOutside = (event) => {
        if (editDialogRef.current && !editDialogRef.current.contains(event.target)) {
            setShowEditDialog(false);
        }
    };

    useEffect(() => {
        if (showEditDialog) {
            document.addEventListener("mousedown", handleClickOutside);
        } else {
            document.removeEventListener("mousedown", handleClickOutside);
        }
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [showEditDialog]);

    const handleCommentClick = (commentId) => {
        setSelectedCommentId(commentId);
    };

    const handleCommentChange = (postId, content) => {
        setCommentContents(prevState => ({
            ...prevState,
            [postId]: content
        }));
    };

    return (
        <div>
            <h2>Posts</h2>
            <div>
                <input
                    type="text"
                    placeholder="Write a new post..."
                    onKeyDown={(e) => {
                        if (e.key === 'Enter') {
                            createPost(e.target.value);
                            e.target.value = '';
                        }
                    }}
                />
            </div>
            <ul>
                {posts.map(post => (
                    <li key={post.id} className="post">
                        <div className="post-header">
                            <p>Posted by {post.author} {new Date(post.creationDate).toLocaleString()}</p>
                        </div>
                        <div className="post-content">
                            <p>{post.content}</p>
                        </div>
                        <div className="post-info">
                            <img
                                className={`upvote-icon ${post.upvoted ? 'active' : ''}`}
                                src={upvoteIcon}
                                alt="An arrow pointing up"
                                onClick={() => handleUpvote(post.id)}
                                style={{ pointerEvents: post.voted ? 'none' : 'auto' }}
                            />
                            {post.upvotes}
                            <img
                                className={`downvote-icon ${post.downvoted ? 'active' : ''}`}
                                src={downvoteIcon}
                                alt="An arrow pointing down"
                                onClick={() => handleDownvote(post.id)}
                                style={{ pointerEvents: post.voted ? 'none' : 'auto' }}
                            />
                            {post.downvotes}
                        </div>
                        <div className="post-footer">
                            <input
                                type="text"
                                placeholder="Write a comment..."
                                value={commentContents[post.id] || ""}
                                onChange={(e) => handleCommentChange(post.id, e.target.value)}
                                onKeyDown={(e) => {
                                    if (e.key === 'Enter') {
                                        createPost(commentContents[post.id], post.id);
                                        handleCommentChange(post.id, "");
                                    }
                                }}
                            />
                            <button onClick={() => {
                                createPost(commentContents[post.id], post.id);
                                handleCommentChange(post.id, "");
                            }}>Comment</button>
                            <button>Share</button>
                            {post.author === currentUser && (
                                <>
                                    <button onClick={() => handleEditClick(post)}>Edit</button>
                                    <button onClick={() => deletePost(post.id)}>Delete</button>
                                </>
                            )}
                        </div>
                        <ul className="comments-list">
                            {post.responses && post.responses.map(response => (
                                <li key={response.id} className="comment" onClick={() => handleCommentClick(response.id)}>
                                    <div className="comment-header">
                                        <p>Comment by {response.author} {new Date(response.creationDate).toLocaleString()}</p>
                                        <button className="report-button">Report</button>
                                    </div>
                                    <div className="comment-content">
                                        <p>{response.content}</p>
                                    </div>
                                    {selectedCommentId === response.id && (
                                        <div className="comment-footer">
                                            <button>Edit</button>
                                            <button>Delete</button>
                                        </div>
                                    )}
                                </li>
                            ))}
                        </ul>
                    </li>
                ))}
            </ul>
            {showEditDialog && (
                <div className="dialog-backdrop">
                    <div className="dialog" ref={editDialogRef}>
                        <h2>Edit Post</h2>
                        <input
                            type="text"
                            value={editPostContent}
                            onChange={(e) => setEditPostContent(e.target.value)}
                        />
                        <button className="save-button" onClick={handleEditConfirm}>Save</button>
                        <button className="cancel-button" onClick={() => setShowEditDialog(false)}>Cancel</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Post;
