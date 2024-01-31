import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { CreateTasksService } from "../../httpService/tasksServices";

export default function CreateTask() {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [status, setStatus] = useState("");
  const [message, setMessage] = useState("");
  const [isSuccess, setIsSuccess] = useState(false);
  const [isLogin, setIsLogin] = useState(false);
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const createTaskDetails = {
        title,
        description,
        status,
      };
      var response = await CreateTasksService(createTaskDetails);
      setIsSuccess(true);
      setMessage(response.data.message);
      setTimeout(() => {
        navigate("/");
      }, 1000);
    } catch (error) {
      setMessage(error.response.data.message);
    }
  };

  useEffect(() => {
    const userId = localStorage.getItem("userId");
    if (userId == "0") {
      navigate("/login");
    } else {
      setIsLogin(true);
    }
  }, [isLogin]);

  return (
    isLogin && (
      <div className="min-h-screen flex items-center justify-center">
        <div className="bg-teal-950 max-w-md w-full p-6 rounded-md shadow-md">
          <h2 className="text-2xl font-semibold mb-6">Create Task</h2>
          {message &&
            (isSuccess ? (
              <div className="mb-4 text-green-600">{message}</div>
            ) : (
              <div className="mb-4 text-red-600">{message}</div>
            ))}
          <form onSubmit={handleLogin}>
            <label className="block mb-4">
              <span className="text-white-700">Title:</span>
              <input
                type="text"
                value={title}
                onChange={(e) => setTitle(e.target.value)}
                className="text-black mt-1 p-2 block w-full rounded-md border-gray-300"
              />
            </label>
            <label className="block mb-4">
              <span className="text-white-700">Description:</span>
              <input
                type="text"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                className="text-black mt-1 p-2 block w-full rounded-md border-gray-300"
              />
            </label>
            <label className="block mb-4">
              <span className="text-white-700">Status:</span>
              <input
                type="text"
                value={status}
                onChange={(e) => setStatus(e.target.value)}
                className="text-black mt-1 p-2 block w-full rounded-md border-gray-300"
              />
            </label>
            <button
              type="submit"
              className="bg-indigo-500 text-white px-4 py-2 rounded-md hover:bg-indigo-600"
            >
              Create
            </button>
          </form>
          <div className="mt-4">
            Don&apos;t want to create new task?{" "}
            <Link to="/" className="text-blue-500 hover:underline">
              task List
            </Link>
          </div>
        </div>
      </div>
    )
  );
}
