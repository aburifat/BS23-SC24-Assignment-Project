import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { LoginUserService } from "../../httpService/authServices";

export default function Login() {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [message, setMessage] = useState("");
  const [isSuccess, setIsSuccess] = useState(false);
  const [isLogin, setIsLogin] = useState(true);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const loginDetails = {
        userName,
        password,
      };
      var response = await LoginUserService(loginDetails);
      const payload = response.data;
      localStorage.setItem("userId", payload.id);
      localStorage.setItem("userName", payload.userName);
      localStorage.setItem("email", payload.email);
      localStorage.setItem("userRole", payload.userRole);
      localStorage.setItem("userRoleName", payload.userRoleName);
      localStorage.setItem("accessToken", payload.accessToken);
      setIsSuccess(true);
      setMessage(response.data.message);
      setTimeout(() => {
        navigate("/");
      }, 2000);
    } catch (error) {
      setMessage(error.response.data.message);
    }
  };

  useEffect(() => {
    const userId = localStorage.getItem("userId");
    if (userId != "0") {
      navigate("/");
    } else {
      setIsLogin(false);
    }
  });

  return (
    !isLogin && (
      <div className="min-h-screen flex items-center justify-center">
        <div className="bg-teal-950 max-w-md w-full p-6 rounded-md shadow-md">
          <h2 className="text-2xl font-semibold mb-6">Login</h2>
          {message &&
            (isSuccess ? (
              <div className="mb-4 text-green-600">{message}</div>
            ) : (
              <div className="mb-4 text-red-600">{message}</div>
            ))}
          <form onSubmit={handleLogin}>
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
              <span className="text-white-700">Password:</span>
              <input
                type="password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                className="text-black mt-1 p-2 block w-full rounded-md border-gray-300"
              />
            </label>
            <button
              type="submit"
              className="bg-indigo-500 text-white px-4 py-2 rounded-md hover:bg-indigo-600"
            >
              Login
            </button>
          </form>
          <div className="mt-4">
            Don&apos;t have an account?{" "}
            <Link to="/register" className="text-blue-500 hover:underline">
              Register here
            </Link>
          </div>
        </div>
      </div>
    )
  );
}
