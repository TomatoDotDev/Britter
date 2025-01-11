import React, { useState } from "react";
import "../styles.css";

const Login = ({ onLogin, onRegister }) => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const handleSubmit = (e) => {
        e.preventDefault();
        if (username === "User" && password === "password") {
            onLogin();
        } else if (username === "User" && password !== "password") {
            setError("Invalid password");
        } else if (username !== "User" && password === "password") {
            setError("Invalid username");
        } else {
            setError("Invalid username or password");
        }
    };

    return (
        <div className="login-container">
            <form className="login-form" onSubmit={handleSubmit}>
                <h2>Login</h2>
                {error && <p className="error">{error}</p>}
                <input
                    type="text"
                    placeholder="Username"
                    value={username}
                    onChange={(e) => setUsername(e.target.value)}
                />
                <input
                    type="password"
                    placeholder="Password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                <div className="button-container">
                    <button type="submit">Login</button>
                    <button type="button" onClick={onRegister}>Register</button>
                </div>
            </form>
        </div>
    );
};

export default Login;