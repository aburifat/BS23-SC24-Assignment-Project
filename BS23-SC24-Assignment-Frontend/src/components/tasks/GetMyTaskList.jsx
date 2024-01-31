import { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { GetMyTasksService } from "../../httpService/tasksServices";

export default function GetMyTaskList() {
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
      const response = await GetMyTasksService();
      setTasks(response.data);
    } catch (error) {
      console.error("Error fetching tasks:", error);
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
  });

  return (
    <div>
      {isLogin && (
        <>
          <div>Task List:</div>
          <ul>
            {tasks.map((task) => (
              <li key={task.id}>{task.title}</li>
            ))}
          </ul>
          <Link to="/tasks/create">
            <button>Create Task</button>
          </Link>
          <Link to="/tasks/update/1">
            {" "}
            {/* Replace with the appropriate task ID */}
            <button>Update Task</button>
          </Link>
          <button onClick={handleLogout}>Logout</button>
        </>
      )}
    </div>
  );
}
