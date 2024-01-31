import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import {
  DeleteTasksService,
  GetAllTasksService,
} from "../../httpService/tasksServices";

export default function GetTaskList() {
  const [isLogin, setIsLogin] = useState(false);
  const [tasks, setTasks] = useState([]);
  const navigate = useNavigate();

  const handleLogout = () => {
    const confirmLogout = window.confirm("Are you sure you want to log out?");
    if (confirmLogout) {
      localStorage.setItem("userId", "0");
      localStorage.removeItem("accessToken");
      setIsLogin(false);
      navigate("/login");
    }
  };

  const fetchTasks = async () => {
    try {
      const response = await GetAllTasksService();
      setTasks(response.data);
    } catch (error) {
      console.error("Error fetching tasks:", error);
    }
  };

  const handleDeleteTask = async (taskId) => {
    const confirmDelete = window.confirm(
      "Are you sure you want to delete this task?"
    );
    if (confirmDelete) {
      try {
        await DeleteTasksService(taskId);
        fetchTasks();
      } catch (error) {
        console.error("Error deleting task:", error);
      }
    }
  };

  useEffect(() => {
    const userId = localStorage.getItem("userId");
    if (userId != "0") {
      setIsLogin(true);
      fetchTasks();
    } else {
      navigate("/login");
    }
  }, [isLogin]);

  return (
    <div className="min-h-screen flex flex-col items-center justify-center">
      {isLogin && (
        <>
          <h2 className="text-2xl font-semibold mb-4">Task List (Admin)</h2>
          <table className="border-collapse border w-[95%] mb-4">
            <thead>
              <tr className="bg-teal-950">
                <th className="border p-2">Title</th>
                <th className="border p-2">Description</th>
                <th className="border p-2">Status</th>
                <th className="border p-2">UserId</th>
                <th className="border p-2">Actions</th>
              </tr>
            </thead>
            <tbody>
              {tasks.map((task) => (
                <tr key={task.id} className="bg-teal-800">
                  <td className="border p-2">{task.title}</td>
                  <td className="border p-2">{task.description}</td>
                  <td className="border p-2">{task.status}</td>
                  <td className="border p-2">{task.userId}</td>
                  <td className="border p-2">
                    <Link to={`/update/${task.id}`}>
                      <button className="bg-blue-500 text-white px-2 py-1 mr-2 hover:bg-blue-600">
                        Update
                      </button>
                    </Link>
                    <button
                      onClick={() => handleDeleteTask(task.id)}
                      className="bg-red-500 text-white px-2 py-1 hover:bg-red-600"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <div className="flex justify-end">
            <Link to="/">
              <button className="bg-blue-500 text-white px-4 py-2 mr-2 hover:bg-blue-600">
                My Tasks
              </button>
            </Link>
            <Link to="/create">
              <button className="bg-green-700 text-white px-4 py-2 mr-2 hover:bg-green-600">
                Create Task
              </button>
            </Link>
            <button
              onClick={handleLogout}
              className="bg-red-500 text-white px-4 py-2 hover:bg-red-600"
            >
              Logout
            </button>
          </div>
        </>
      )}
    </div>
  );
}
