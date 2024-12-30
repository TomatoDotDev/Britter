import React, { useState } from "react";
import Login from "./components/Login";
import MainPage from "./components/MainPage";
import "./styles.css";

const App = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    return (
        <div>
            {isAuthenticated ? (
                <MainPage onLogout={() => setIsAuthenticated(false)} />
            ) : (
                <Login onLogin={() => setIsAuthenticated(true)} />
            )}
        </div>
    );
};

export default App;