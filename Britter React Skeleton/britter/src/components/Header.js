import React from "react";
import "../styles.css";
import britterIcon from "../Icons/BritterIconTransparent.png";

const Header = ({ onLogout }) => {
    const handleLogout = async () => {
        try {
            console.log("Attempting to log out...");
            const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/account/logout`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'text/plain;charset=UTF-8'
                },
                credentials: 'include',
                body: ''
            });

            if (response.ok) {
                console.log("Logout successful");
                onLogout();
            } else {
                console.error("Logout failed", response);
            }
        } catch (error) {
            console.error("An error occurred while trying to log out", error);
        }
    };

    return (
        <header className="header">
            <div className="header-logo">
                <img
                    className="britter-icon"
                    src={britterIcon}
                    alt="The Britter company logo"
                />
                <div className="title">Britter</div>
            </div>
            <nav>
                <button className="logout-button" onClick={handleLogout}>Sign Out</button>
            </nav>
        </header>
    );
};

export default Header;