import React, { useState, useEffect } from "react";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";

function Navbar() {
    const [username, setUsername] = useState("Guest");
    const [email, setEmail] = useState("No email");
    const [isDropdownOpen, setIsDropdownOpen] = useState(false);
    const navigate = useNavigate();

    const logOut = () => {
        localStorage.removeItem("authToken");
        setUsername("Guest");
        setEmail("No email");
        setIsDropdownOpen(false);
        navigate("/login");
    }

    useEffect(() => {
        const token = localStorage.getItem('authToken');
        if (token) {
            try {
                const decodedToken = jwtDecode(token);
                setUsername(decodedToken.Name || "Guest");
                setEmail(decodedToken.Email || "No email");
            } catch(error) {
                console.error("Error decoding token:", error);
            }
        }
    }, []);

    const toggleDropdown = () => {
        setIsDropdownOpen((prev) => !prev);
    };

    return (
        <nav className="bg-white border-gray-200 dark:bg-gray-900">
            <div className="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4">
                <a href="/" className="flex items-center space-x-3">
                    <img onClick={() => navigate("/")} src="src\Website Resources\medical-team.png" className="h-8" alt="Logo" />
                    <span className="text-2xl font-semibold dark:text-white">Beacon of Hope Clinic</span>
                </a>

                {/* User Icon and Dropdown */}
                <div className="relative">
                    <button
                        type="button"
                        className="flex text-sm bg-gray-800 rounded-full focus:ring-4 focus:ring-gray-300 dark:focus:ring-gray-600"
                        onClick={toggleDropdown}
                    >
                        <img className="w-8 h-8 rounded-full" src="src\Website Resources\user.png" alt="user photo" />
                    </button>

                    {/* Dropdown menu */}
                    {isDropdownOpen && (
                        <div className="absolute right-0 mt-2 w-48 bg-white border rounded-lg shadow-lg dark:bg-gray-700">
                            <div className="px-4 py-3">
                                <span className="block text-sm text-gray-900 dark:text-white">{username}</span>
                                <span className="block text-sm text-gray-500 truncate dark:text-gray-400">{email}</span>
                            </div>
                            <ul className="py-2">
                                <li>
                                    <a href="#" className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200">Appointments</a>
                                </li>
                                {/* <li>
                                    <a href="#" className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200">Settings</a>
                                </li>
                                <li>
                                    <a href="#" className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200">Earnings</a>
                                </li> */}
                                <li>
                                    <a onClick={logOut} href="#" className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200">Sign out</a>
                                </li>
                            </ul>
                        </div>
                    )}
                </div>
            </div>
        </nav>
    );
}

export default Navbar;
