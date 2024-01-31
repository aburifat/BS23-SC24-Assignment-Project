import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { RegisterUserService } from "../../httpService/authServices";

export default function Register() {
  const [userName, setUserName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [message, setMessage] = useState("");
  const [isSuccess, setIsSuccess] = useState(false);
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();

    try {
      const registerDetails = {
        userName,
        email,
        password,
        confirmPassword,
      };
      var response = await RegisterUserService(registerDetails);
      setIsSuccess(true);
      setMessage(response.data.message);
      setTimeout(() => {
        navigate("/login");
      }, 2000);
    } catch (error) {
      setMessage(error.response.data.message);
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center">
      <div className="max-w-md w-full p-6 rounded-md shadow-md">
        <h2 className="text-2xl font-semibold mb-6">Register</h2>
        {message &&
          (isSuccess ? (
            <div className="mb-4 text-green-600">{message}</div>
          ) : (
            <div className="mb-4 text-red-600">{message}</div>
          ))}
        <form onSubmit={handleRegister}>
          <label className="block mb-4">
            <span className="text-white-700">Username:</span>
            <input
              type="text"
              value={userName}
              onChange={(e) => setUserName(e.target.value)}
              className="text-black mt-1 p-2 block w-full rounded-md border-gray-300"
            />
          </label>
          <label className="block mb-4">
            <span className="text-white-700">Email:</span>
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              className="text-black mt-1 p-2 block w-full rounded-md border-gray-300"
            />
          </label>
          <label className="block mb-4">
            <span className="text-white-700">Password:</span>
            <input
              type="password"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              className="text-black mt-1 p-2 block w-full rounded-md border-gray-300"
            />
          </label>
          <label className="block mb-4">
            <span className="text-white-700">Confirm Password:</span>
            <input
              type="password"
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              className="text-black mt-1 p-2 block w-full rounded-md border-gray-300"
            />
          </label>
          <button
            type="submit"
            className="bg-indigo-500 text-white px-4 py-2 rounded-md hover:bg-indigo-600"
          >
            Register
          </button>
        </form>
      </div>
    </div>
  );
}
