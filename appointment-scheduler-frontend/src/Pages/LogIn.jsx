import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

function LogIn() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [message, setMessage] = useState("");

    const navigate = useNavigate();

    const handleLogIn = async (e) => {
        e.preventDefault();
        try {
            const res = await fetch("https://localhost:7153/api/Users/login", {
                method: "POST",
                mode: "cors",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/json",
                },
                body: JSON.stringify({ email, password }),
            });

            if (!res.ok) {
                throw new Error(`HTTP error! status: ${res.status}`);
            }

            const data = await res.json();

            if (data.token) {
                localStorage.setItem("authToken", data.token);
                console.log("Saved token: ", localStorage.getItem("authToken")); // Debugging purposes
                const decodedToken = jwtDecode(localStorage.getItem("authToken"));
                console.log(decodedToken);
                setMessage(data.message);
                navigate("/"); // Navigate to dashboard after successful login
            }
        } catch (error) {
            console.error("Error while logging in", error);
            setMessage("Failed to log in. Please check your credentials.");
        }
    };

    return (
        <div className="p-10">
            <h1 className="mb-8 font-extrabold text-4xl">Log In</h1>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                <form onSubmit={handleLogIn}>
                    <div className="mt-4">
                        <label className="block font-semibold" htmlFor="email">
                            Email
                        </label>
                        <input
                            className="w-full shadow-inner bg-gray-100 rounded-lg placeholder-black text-2xl p-4 border-none block mt-1"
                            id="email"
                            type="email"
                            name="email"
                            required
                            onChange={(e) => setEmail(e.target.value)}
                        />
                    </div>

                    <div className="mt-4">
                        <label className="block font-semibold" htmlFor="password">
                            Password
                        </label>
                        <input
                            className="w-full shadow-inner bg-gray-100 rounded-lg placeholder-black text-2xl p-4 border-none block mt-1"
                            id="password"
                            type="password"
                            name="password"
                            required
                            autoComplete="new-password"
                            onChange={(e) => setPassword(e.target.value)}
                        />
                    </div>

                    <div className="flex items-center justify-between mt-8">
                        <button
                            type="submit"
                            className="flex items-center justify-center px-8 py-3 border border-transparent text-base font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 md:py-4 md:text-lg md:px-10"
                            onClick={() => navigate("/")}                       
                        >
                            Log in
                        </button>
                        <a
                            className="font-semibold cursor-pointer"
                            onClick={() => navigate("/signup")}
                        >
                            Not registered?
                        </a>
                    </div>
                </form>

                <aside>
                    <div className="bg-gray-100 p-8 rounded">
                        <h2 className="font-bold text-2xl">Instructions</h2>
                        <ul className="list-disc mt-4 list-inside">
                            <li>
                                All users must provide a valid email address and password to create an
                                account.
                            </li>
                            <li>
                                Users must not use offensive, vulgar, or otherwise inappropriate
                                language in their username or profile information.
                            </li>
                            <li>Users must not create multiple accounts for the same person.</li>
                        </ul>
                    </div>
                </aside>
            </div>
            {message && <p className="mt-4 text-red-500">{message}</p>}
        </div>
    );
}

export default LogIn;