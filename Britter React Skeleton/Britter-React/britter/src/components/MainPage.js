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
                    title="First post" 
                    username="u/Liam" 
                    time="1 hour ago" 
                    content="This is the first post! Pretty cool right?"
                />
                <Post 
                    title="Post Title 2" 
                    username="u/anotheruser" 
                    time="30 mins ago" 
                    content="Here is another example of a post."
                />
            </main>
            <Footer />
        </div>
    );
};

export default MainPage;