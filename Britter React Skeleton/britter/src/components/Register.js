import React, { useState } from "react";
import "../styles.css";

const Register = ({ onRegister }) => {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState("");
    const [showConfirmPopup, setShowConfirmPopup] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (password !== confirmPassword) {
            setError("Passwords do not match");
        } else if (email === "" || password === "" || confirmPassword === "") {
            setError("Please fill in all fields");
        } else {
            setShowConfirmPopup(true);
        }
    };

    const handleConfirm = async () => {
        setShowConfirmPopup(false);
        try {
            const response = await fetch(`${process.env.REACT_APP_API_ADDRESS}/register`, {
                method: 'POST',
                headers: {
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
    };

    const handleCancel = () => {
        setShowConfirmPopup(false);
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
                <button type="button" onClick={() => onRegister()}>Cancel</button>
            </form>
            {showConfirmPopup && (
                <div className="dialog-backdrop">
                    <div className="dialog">
                        <h2>Code of Conduct</h2>
                        <h3>By registering, you agree to follow the Code of Conduct below.</h3>
                        <p></p>
                        <p>1. Be respectful and considerate to others.</p>
                        <p>2. Do not post harmful or offensive content.</p>    
                        <p>3. Do not engage in harassment or bullying.</p>
                        <p>4. Do not share personal information.</p>
                        <h3>Do you accept the Code of Conduct?</h3>
                        <button className="save-button" onClick={handleConfirm}>Accept</button>
                        <button className="cancel-button" onClick={handleCancel}>Deny</button>
                    </div>
                </div>
            )}
        </div>
    );
};

export default Register;