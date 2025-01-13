import React, { useState } from "react";
import "../styles.css";

const Register = ({ onRegister }) => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState("");

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (password !== confirmPassword) {
            setError("Passwords do not match");
        } else if (email === "" || password === "" || confirmPassword === "") {
            setError("Please fill in all fields");
        } else {
            try {
                const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/register`, {
                    method: 'POST',
                    headers: {
                        Authorization: 'Bearer test-bearer-token',
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        email: email,
                        password: password
                    })
                });

                if (response.ok) {
                    onRegister();
                } else {
                    const errorData = await response.json();
                    setError(errorData.message || "Registration failed"); // errorData.message is the message from the server - Needs to be added to API
                }
            } catch (error) {
                setError("An error occurred during registration");
            }
        }
    };

    return (
        <div className="register-container">
            <form className="register-form" onSubmit={handleSubmit}>
                <h2>Register</h2>
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
                <input
                    type="password"
                    placeholder="Confirm Password"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                />
                <button type="submit">Register</button>
            </form>
        </div>
    );
};

export default Register;