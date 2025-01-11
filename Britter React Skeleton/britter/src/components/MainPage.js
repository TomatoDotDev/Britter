import React from "react";
import Header from "./Header";
import Post from "./Post";
import Footer from "./Footer";
import "../styles.css";

const MainPage = ({ onLogout }) => {
    return (
        <div>
            <Header onLogout={onLogout} />
            <main>
                <Post 
                    username="u/Liam" 
                    time="1 hour" 
                    content="This is the first post! Pretty cool right?"
                />
                <Post 
                    username="u/anotheruser" 
                    time="30 mins" 
                    content="Here is another example of a post."
                />
                <Post
                    username="Username from database"
                    time="Time from database"
                    content="Content from database"
                />
            </main>
            <Footer />
        </div>
    );
};

export default MainPage;