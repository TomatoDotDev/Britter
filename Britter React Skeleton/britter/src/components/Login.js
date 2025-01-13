import React, { useState } from "react";
import "../styles.css";

const Login = ({ onLogin, onRegister }) => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (email === "" || password === "") {
            setError("Please fill in all fields.");
        } else {
            try {
                const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/login`, {
                    method: 'POST',
                    headers: {
                        Authorization: 'Bearer test-bearer-token',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        email: email,
                        password: password,
                        twoFactorCode: null,
                        twoFactorRecoveryCode: null
                    })
                })

                if (response.ok) {
                    onLogin();
                } else {
                    const errorData = await response.json();
                    setError(errorData.message || "Login failed"); // errorData.message is the message from the server - Needs to be added to API
                }
            } catch (error) {
                setError("An error occurred while trying to log in");
            }
        }
    };

    return (
        <div className="login-container">
            <form className="login-form" onSubmit={handleSubmit}>
                <h2>Login</h2>
                {error && <p className="error">{error}</p>}
                <input
                    type="text"
                    placeholder="Email"
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
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