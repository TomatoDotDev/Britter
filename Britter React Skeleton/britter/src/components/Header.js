import React from "react";
import "../styles.css";

const Header = ({ onLogout }) => {
    return (
        <header className="header">
            <image src="../../public/Britter.png"/>
            <div className="logo">Britter</div>
            <nav>
                <button className="logout-button" onClick={onLogout}>Sign Out</button>
            </nav>
        </header>
    );
};

export default Header;
