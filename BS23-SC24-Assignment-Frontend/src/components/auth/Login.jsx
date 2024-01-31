import { useState } from "react";
import { Navigate } from "react-router";
import { LoginUserService } from "../../httpService/authServices";

export default function Login() {
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      const loginDetails = {
        userName,
        password,
      };
      var data = LoginUserService(loginDetails);
      const payload = (await data).data;
      localStorage.setItem("accessToken", payload.accessToken);
      Navigate("/home", { replace: true });
    } catch (error) {
      console.error("Login failed: ", error);
    }
  };

  return (
    <div>
      <h2>Login</h2>
      <form onSubmit={handleLogin}>
        <label>
          Username:
          <input
            type="text"
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
          />
        </label>
        <br />
        <label>
          Password:
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </label>
        <br />
        <button type="submit">Login</button>
      </form>
    </div>
  );
}
