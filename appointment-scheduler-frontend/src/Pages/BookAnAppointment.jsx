import React, { useState, useEffect } from "react";
import Success from "./Success";
import { jwtDecode } from "jwt-decode";
import { useNavigate } from "react-router-dom";
import { DemoContainer } from '@mui/x-date-pickers/internals/demo';
import dayjs from "dayjs";
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import axios from "axios";

function BookAnAppointment() {
    const [fullname, setFullName] = useState("");
    const [email, setEmail] = useState("");
    const [userId, setUserId] = useState("");
    const [appointmentTitle, setAppointmentTitle] = useState("");
    const [selectedDate, setSelectedDate] = useState(null);
    const [selectedTime, setSelectedTime] = useState("");
    const [doctor, setDoctor] = useState(null);
    const [doctors, setDoctors] = useState([]);
    const [appointmentTypes, setAppointmentTypes] = useState([]);
    const [error, setError] = useState("");
    const [timestamp, setTimestamp] = useState([]);
    const [message, setMessage] = useState("");
    const navigate = useNavigate();
    const baseURL = `https://localhost:7153/`;

    const formatDate = (date) => {
        if (!date) return null;

        if (!dayjs(date).isValid()) return null;
        
        return dayjs(date).format("YYYY-MM-DD");
    }

    const formatTime = (time) => {
        if (!time) return null;
        return dayjs(time, "HH:mm").format("HH:mm");
    }

    const sendEmail = async() => {

        const formattedDate = formatDate(selectedDate);
        const formattedTime = formatTime(selectedTime);

        const payload = {
            sendTo: email,
            messageBody: `
        <!DOCTYPE html>
        <html>
        <body style="font-family: Arial, sans-serif; line-height: 1.6; color: #333;">
            <div style="max-width: 600px; margin: 0 auto; padding: 20px;">
                <h2 style="color: #2c5282;">Appointment Confirmation</h2>
                
                <p>Dear ${fullname},</p>
                
                <p>Thank you for choosing Beacon of Hope Clinic. This email confirms your upcoming appointment details:</p>
                
                <div style="background-color: #f7fafc; padding: 15px; border-radius: 5px; margin: 20px 0;">
                    <p style="margin: 5px 0;">📅 <strong>Date:</strong> ${formattedDate}</p>
                    <p style="margin: 5px 0;">🕒 <strong>Time:</strong> ${formattedTime}</p>
                    <p style="margin: 5px 0;">📍 <strong>Location:</strong> Beacon of Hope Clinic</p>
                </div>
                
                <h3 style="color: #2c5282;">Important Notes:</h3>
                <ul style="padding-left: 20px;">
                    <li>Please arrive 15 minutes before your scheduled appointment</li>
                    <li>Bring any relevant medical records or test results</li>
                    <li>If you need to reschedule, please contact us 24 hours in advance</li>
                </ul>
                
                <p>Should you have any questions or concerns, please don't hesitate to contact us.</p>
                
                <p>Best regards,<br>
                The Beacon of Hope Clinic Team</p>
                
                <hr style="border: 1px solid #edf2f7; margin: 20px 0;">
                
                <div style="font-size: 14px; color: #666;">
                    <p>📞 Phone: (011) 123-4567<br>
                    ✉️ Email: contact@beaconofhope.com</p>
                </div>
            </div>
        </body>
        </html>
            `.trim()
        };

        try {
            const res = await fetch(`${baseURL}api/EmailService/send-email`, {
                method: "POST",
                mode: "cors",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                },
                body: JSON.stringify(payload)
            })

            if (!res.ok) {
                throw new Error(`HTTP error! Status: ${res.status}`);
            }

        } catch(error) {
            console.error("Error occured while sending email", error);
            setError("Failed to send email");
        }
    }

    const createAppointment = async(e) => {
        e.preventDefault();

        const formattedDate = formatDate(selectedDate);
        const formattedTime = formatTime(selectedTime);

        if (!formattedDate || !formattedDate || !appointmentTitle) {
            setError("Please fill in all fields");
            return;
        }

        //Debugging purposes
        const payload = {
            title: appointmentTitle, 
            userId: Number(userId),
            doctorId: 1, 
            appointmentDate: formattedDate,
            appointmentTime: formattedTime,
            status: "Pending", 
        };

        try {
            const res = await fetch(`${baseURL}api/Appointment`, {
                method: "POST",
                mode: "cors",
                credentials: "same-origin",
                headers: {
                    "Content-Type": "application/json",
                    "Accept": "application/json"
                },
                body: JSON.stringify(payload)
            });

            console.log(`Formatted date: ${formattedDate}`);
            console.log(`Formatted time: ${formattedTime}`)
            console.log(payload);

            if (!res.ok) {
                throw new Error(`HTTP error! Status: ${res.status}`);
            }

            navigate('/success');
            sendEmail();

        } catch (error) {
            console.error("Error occurred while posting data", error);
            setError("Failed to book appointment");
        }
    }

    // const sendEmail = async(e) => {
    //     e.preventDefault;

    //     const formattedDate = formatDate(selectedDate);
    //     const formattedTime = formatTime(selectedTime);

    //     const payload = {
    //         receipient: email,
    //         message: `Hi ${fullname}, this email confirms appointment made at Beacon of Hope Clinic for ${appointmentTitle} at ${formattedTime} on ${formattedDate}. Please arrive 15 minutes before the scheduled time. `
    //     }

    //     try {
    //         const res = await fetch(`${baseURL}api/EmailService/send-email`, {
    //             method: "POST",
    //             mode: "cors",
    //             credentials: "same-origin",
    //             headers: {
    //                 "Content-Type": "application/json",
    //                 "Accept": "application/json"
    //             },
    //             body: JSON.stringify(payload)
    //         })
    //     } catch(error) {
    //         console.error("Error occured while sending email", error);
    //         setError("Failed to send email");
    //     }
    // }

    useEffect(() => {
        const token = localStorage.getItem("authToken");
        if (token) {
            try {
                const decodedToken = jwtDecode(token);
                setEmail(decodedToken.Email);
                setFullName(decodedToken.Name);
                setUserId(decodedToken.Id);
            } catch(error) {
                console.error("Error decoding token", error);
            }
        }
    }, []);

    useEffect(() => {
        axios.get(`${baseURL}api/AppointmentTypes`)
        .then(response => setAppointmentTypes(response.data))
        .catch(err => setError(err.message));
    }, []);

    useEffect(() => {
        axios.get(`${baseURL}api/Timestamp`)
        .then(response => setTimestamp(response.data))
        .catch(err => setError(err.message));
    }, []);

    return (
        <div className="mx-14 mt-10 border-2 border-blue-400 rounded-lg">
            <div className="mt-10 text-center font-bold">Contact Us</div>
            <div className="mt-3 text-center text-4xl font-bold">Make an Appointment</div>
            <div className="p-8">
                <div className="flex gap-4">
                    <input 
                        value={fullname} 
                        type="text" 
                        name="name" 
                        className="mt-1 block w-1/2 rounded-md border border-slate-300 bg-white px-3 py-4 placeholder-slate-400 shadow-sm placeholder:font-semibold placeholder:text-gray-500 focus:border-sky-500 focus:outline-none focus:ring-1 focus:ring-sky-500 sm:text-sm" 
                        placeholder="Full Name *" 
                        readOnly 
                    />
                    <input 
                        value={email} 
                        type="email" 
                        name="email" 
                        className="mt-1 block w-1/2 rounded-md border border-slate-300 bg-white px-3 py-4 placeholder-slate-400 shadow-sm placeholder:font-semibold placeholder:text-gray-500 focus:border-sky-500 focus:outline-none focus:ring-1 focus:ring-sky-500 sm:text-sm" 
                        placeholder="Email *" 
                        readOnly 
                    />
                </div>
                <div className="my-6 flex gap-4">
                    <select 
                        onChange={(e) => setAppointmentTitle(e.target.value)} 
                        name="select" 
                        id="titleselect" 
                        className="block w-1/2 rounded-md border border-slate-300 bg-white px-3 py-4 font-semibold text-gray-300 shadow-sm focus:border-sky-500 focus:outline-none focus:ring-1 focus:ring-sky-500 sm:text-sm"
                    >
                        <option value="">Select Appointment Type</option>
                        {appointmentTypes.map((type) => (
                            <option key={type.id} value={type.name}>
                                {type.name}
                            </option>
                        ))}
                    </select>
                    <select 
                        onChange={(e) => setSelectedTime(e.target.value)} 
                        name="time" 
                        id="select" 
                        className="block w-1/2 rounded-md border border-slate-300 bg-white px-3 py-4 font-semibold text-gray-300 shadow-sm focus:border-sky-500 focus:outline-none focus:ring-1 focus:ring-sky-500 sm:text-sm"
                    >
                        <option value="">Select Time</option>
                        {timestamp.map((time) => (
                            <option key={time.id} value={time.timestamps}>
                                {time.timestamps}
                            </option>
                        ))}
                    </select>
                </div>
                <div>
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <DemoContainer components={['DatePicker']}>
                            <DatePicker 
                                label="Appointment Date"
                                value={selectedDate}
                                onChange={(newValue) => setSelectedDate(newValue)} 
                            />
                        </DemoContainer>
                    </LocalizationProvider>
                </div>
                {error && <div className="text-red-500 text-center my-4">{error}</div>}
                <div className="text-center mt-6">
                    <button 
                        onClick={createAppointment}
                        className="rounded-lg bg-blue-700 px-8 py-5 text-sm font-semibold text-white cursor-pointer"
                    >
                        Book Appointment
                    </button>
                </div>
            </div>
        </div>
    );
}

export default BookAnAppointment;