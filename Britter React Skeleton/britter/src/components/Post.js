import React, { useState } from "react";
import "../styles.css";
import upvoteIcon from "../Icons/UpvoteIcon.svg";
import downvoteIcon from "../Icons/DownvoteIcon.svg";

const Post = ({ username, time, content }) => {
    const [voteCount, setVoteCount] = useState(0);
    const [upvoted, setUpvoted] = useState(false);
    const [downvoted, setDownvoted] = useState(false);

    const handleUpvote = () => {
        if (!upvoted) {
            setVoteCount(voteCount + 1);
            setUpvoted(true);
            if (downvoted) {
                setDownvoted(false);
                setVoteCount(voteCount + 2);
            }
        }
    };

    const handleDownvote = () => {
        if (!downvoted) {
            setVoteCount(voteCount - 1);
            setDownvoted(true);
            if (upvoted) {
                setUpvoted(false);
                setVoteCount(voteCount - 2);
            }
        }
    };

    return (
        <div className="post">
            <div className="post-header">
                <p>Posted by {username} {time} ago</p>
            </div>
            <div className="post-content">
                <p>{content}</p>
            </div>
            <div className="post-info">
                <img
                    className={`upvote-icon ${upvoted ? "active" : ""}`}
                    src={upvoteIcon}
                    alt="An arrow pointing up"
                    onClick={handleUpvote}
                />
                {voteCount}
                <img
                    className={`downvote-icon ${downvoted ? "active" : ""}`}
                    src={downvoteIcon}
                    alt="An arrow pointing down"
                    onClick={handleDownvote}
                />
            </div>
            <div className="post-footer">
                <button>Comment</button>
                <button>Share</button>
            </div>
        </div>
    );
};

export default Post;
