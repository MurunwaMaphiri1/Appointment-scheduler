import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import HomePage from "./Pages/HomePage";
import SignUp from "./Pages/SignUp";
import LogIn from "./Pages/LogIn";
import Navbar from "./Components/Navbar";
import BookAnAppointment from "./Pages/BookAnAppointment";
import Success from "./Pages/Success";

function App() {


  return (
    <>
      <Router>
      <Navbar/>
          <Routes>
            <Route path="/signup" element={<SignUp/>}/>
            <Route path="/login" element={<LogIn/>}/>
            <Route path="/" element={<HomePage/>}/>
            <Route path="/bookanappointment" element={<BookAnAppointment/>}/>
            <Route path="/success" element={<Success/>}/>
          </Routes>
      </Router>
    </>
  )
}

export default App
