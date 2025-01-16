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
    const [editCommentContent, setEditCommentContent] = useState("");
    const [editCommentId, setEditCommentId] = useState(null);
    const [showEditCommentDialog, setShowEditCommentDialog] = useState(false);
    const [reportReason, setReportReason] = useState("");
    const [reportCommentId, setReportCommentId] = useState(null);
    const [showReportDialog, setShowReportDialog] = useState(false);
    const [errorMessage, setErrorMessage] = useState("");
    const [showErrorDialog, setShowErrorDialog] = useState(false);
    const editDialogRef = useRef(null);
    const editCommentDialogRef = useRef(null);
    const reportDialogRef = useRef(null);
    const errorDialogRef = useRef(null);

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
                throw new Error('Error fetching user');
            }
        } catch (error) {
            console.error('Error fetching user:', error);
            setErrorMessage('Error fetching user');
            setShowErrorDialog(true);
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
                throw new Error('Error fetching posts');
            }
        } catch (error) {
            console.error('Error fetching posts:', error);
            setErrorMessage('Error fetching posts');
            setShowErrorDialog(true);
        }
    };

    const createPost = async (content, parentPostId = null) => {
        if (!content || !content.trim()) {
            setErrorMessage('Content cannot be empty');
            setShowErrorDialog(true);
            return;
        }
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
                throw new Error('Error creating post');
            }
        } catch (error) {
            console.error("Error creating post:", error);
            setErrorMessage('Error creating post');
            setShowErrorDialog(true);
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
                body: JSON.stringify(content)
            });
            if (response.ok) {
                getPosts();
            } else {
                throw new Error('Error editing post');
            }
        } catch (error) {
            console.error("Error editing post:", error);
            setErrorMessage('Error editing post');
            setShowErrorDialog(true);
        }
    };

    const deletePost = async (postId) => {
        try {
            const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/Post/Delete?id=${postId}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'text/plain;charset=UTF-8'
                },
                credentials: 'include',
            });
            if (response.ok) {
                getPosts();
            } else {
                throw new Error('Error deleting post');
            }
        } catch (error) {
            console.error("Error deleting post:", error);
            setErrorMessage('Error deleting post');
            setShowErrorDialog(true);
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
                    throw new Error('Error upvoting post');
                }
            } catch (error) {
                console.error("Error upvoting post:", error);
                setErrorMessage('Error upvoting post');
                setShowErrorDialog(true);
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
                    throw new Error('Error downvoting post');
                }
            } catch (error) {
                console.error("Error downvoting post:", error);
                setErrorMessage('Error downvoting post');
                setShowErrorDialog(true);
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

    const handleEditCommentClick = (comment) => {
        setEditCommentId(comment.id);
        setEditCommentContent(comment.content);
        setShowEditCommentDialog(true);
    };

    const handleEditCommentConfirm = () => {
        editPost(editCommentId, editCommentContent);
        setShowEditCommentDialog(false);
    };

    const handleReportClick = (comment) => {
        setReportCommentId(comment.id);
        setReportReason("");
        setShowReportDialog(true);
    };

    const handleReportConfirm = async () => {
        try {
            const response = await fetch('http://localhost:5297/Report/Create', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                credentials: 'include',
                body: JSON.stringify({
                    postId: reportCommentId,
                    reason: reportReason
                })
            });
            if (response.ok) {
                setShowReportDialog(false);
            } else {
                throw new Error('Error reporting comment');
            }
        } catch (error) {
            console.error("Error reporting comment:", error);
            setErrorMessage('Error reporting comment');
            setShowErrorDialog(true);
        }
    };

    const handleClickOutside = (event) => {
        if (editDialogRef.current && !editDialogRef.current.contains(event.target)) {
            setShowEditDialog(false);
        }
        if (editCommentDialogRef.current && !editCommentDialogRef.current.contains(event.target)) {
            setShowEditCommentDialog(false);
        }
        if (reportDialogRef.current && !reportDialogRef.current.contains(event.target)) {
            setShowReportDialog(false);
        }
        if (errorDialogRef.current && !errorDialogRef.current.contains(event.target)) {
            setShowErrorDialog(false);
        }
    };

    useEffect(() => {
        if (showEditDialog || showEditCommentDialog || showReportDialog || showErrorDialog) {
            document.addEventListener("mousedown", handleClickOutside);
        } else {
            document.removeEventListener("mousedown", handleClickOutside);
        }
        return () => {
            document.removeEventListener("mousedown", handleClickOutside);
        };
    }, [showEditDialog, showEditCommentDialog, showReportDialog, showErrorDialog]);

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
            <h2 className="post-content-title">Posts</h2>
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
                {posts.filter(post => !post.isDeleted).map(post => (
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
                                        if (commentContents[post.id] && commentContents[post.id].trim()) {
                                            createPost(commentContents[post.id], post.id);
                                            handleCommentChange(post.id, "");
                                        } else {
                                            setErrorMessage('Content cannot be empty');
                                            setShowErrorDialog(true);
                                        }
                                    }
                                }}
                            />
                            <button onClick={() => {
                                if (commentContents[post.id] && commentContents[post.id].trim()) {
                                    createPost(commentContents[post.id], post.id);
                                    handleCommentChange(post.id, "");
                                } else {
                                    setErrorMessage('Content cannot be empty');
                                    setShowErrorDialog(true);
                                }
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
                            {post.responses && post.responses.filter(response => !response.isDeleted).map(response => (
                                <li key={response.id} className="comment" onClick={() => handleCommentClick(response.id)}>
                                    <div className="comment-header">
                                        <p>Comment by {response.author} {new Date(response.creationDate).toLocaleString()}</p>
                                        <button className="report-button" onClick={() => handleReportClick(response)}>Report</button>
                                    </div>
                                    <div className="comment-content">
                                        <p>{response.content}</p>
                                    </div>
                                    {selectedCommentId === response.id && (
                                        <div className="comment-footer">
                                            {response.author === currentUser && (
                                                <>
                                                    <button onClick={() => handleEditCommentClick(response)}>Edit</button>
                                                    <button onClick={() => deletePost(response.id)}>Delete</button>
                                                </>
                                            )}
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
            {showEditCommentDialog && (
                <div className="dialog-backdrop">
                    <div className="dialog" ref={editCommentDialogRef}>
                        <h2>Edit Comment</h2>
                        <input
                            type="text"
                            value={editCommentContent}
                            onChange={(e) => setEditCommentContent(e.target.value)}
                        />
                        <button className="save-button" onClick={handleEditCommentConfirm}>Save</button>
                        <button className="cancel-button" onClick={() => setShowEditCommentDialog(false)}>Cancel</button>
                    </div>
                </div>
            )}
            {showReportDialog && (
                <div className="dialog-backdrop">
                    <div className="dialog" ref={reportDialogRef}>
                        <h2>Report Comment</h2>
                        <input
                            type="text"
                            placeholder="Reason for reporting"
                            value={reportReason}
                            onChange={(e) => setReportReason(e.target.value)}
                        />
                        <button className="save-button" onClick={handleReportConfirm}>Report</button>
                        <button className="cancel-button" onClick={() => setShowReportDialog(false)}>Cancel</button>
                    </div>
                </div>
            )}
            {showErrorDialog && (
                <div className="dialog-backdrop">
                    <div className="dialog" ref={errorDialogRef}>
                        <h2>Error</h2>
                        <p>{errorMessage}</p>
                        <button className="ok-button" onClick={() => setShowErrorDialog(false)}>OK</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Post;
