import { Link, useNavigate } from "react-router-dom";

export default function LandingPage() {
  const navigate = useNavigate();
  const handleLogout = () => {
    const confirmLogout = window.confirm("Are you sure you want to log out?");
    if (confirmLogout) {
      localStorage.setItem("userId", "0");
      localStorage.removeItem("accessToken");
      navigate("/login");
    }
  };

  return (
    <div>
      {localStorage.getItem("userId") != "0" ? (
        <div>
          <Link to="/tasks">
            <button>My Tasks</button>
          </Link>
          {localStorage.getItem("userRoleName") === "Administrator" && (
            <Link to="/tasks/all">
              <button>All Tasks</button>
            </Link>
          )}
          <Link>
            <button onClick={handleLogout}>Logout</button>
          </Link>
        </div>
      ) : (
        <div>
          <Link to="/login">
            <button>Login</button>
          </Link>
          <Link to="/register">
            <button>Register</button>
          </Link>
        </div>
      )}
    </div>
  );
}
