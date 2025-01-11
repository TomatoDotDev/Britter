import React, { useState } from "react";
import Login from "./components/Login";
import Register from "./components/Register";
import MainPage from "./components/MainPage";
import "./styles.css";

const App = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [isRegistering, setIsRegistering] = useState(false);

    return (
        <div>
            {isAuthenticated ? (
                <MainPage onLogout={() => setIsAuthenticated(false)} />
            ) : isRegistering ? (
                <Register onRegister={() => setIsRegistering(false)} />
            ) : (
                <Login onLogin={() => setIsAuthenticated(true)} onRegister={() => setIsRegistering(true)} />
            )}
        </div>
    );
};

export default App;