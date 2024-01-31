import { Link } from "react-router-dom";
import { TokenValidationService } from "../../httpService/authServices";

export default function LandingPage() {
  const isTokenValid = async () => {
    var response = await TokenValidationService();
    if (response.status == 200) return true;
    return false;
  };

  return (
    <div>
      {isTokenValid ? (
        <div>
          <Link to="/tasks">
            <button>My Tasks</button>
          </Link>
          {localStorage.getItem("userRoleName") === "Administrator" && (
            <Link to="/tasks/all">
              <button>All Tasks</button>
            </Link>
          )}
          <Link to="/tasks/create">
            <button>Register</button>
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
