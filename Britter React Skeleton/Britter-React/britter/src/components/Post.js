import React from "react";
import "../styles.css";

const Post = ({ title, username, time, content }) => {
    return (
        <div className="post">
            <div className="post-header">
                <h3>{title}</h3>
                <p>Posted by {username} {time} ago</p>
            </div>
            <div className="post-content">
                <p>{content}</p>
            </div>
            <div className="post-footer">
                <button>Upvote</button>
                <button>Comment</button>
                <button>Share</button>
            </div>
        </div>
    );
};

export default Post;
