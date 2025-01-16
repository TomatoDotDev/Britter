import React from "react";
import Header from "./Header";
import Topics from "./Topics";
import "../styles.css";

const MainPage = ({ onLogout }) => {
    return (
        <div>
            <Header onLogout={onLogout} />
            <main className="main-content">
                <Topics />
            </main>
        </div>
    );
};

export default MainPage;