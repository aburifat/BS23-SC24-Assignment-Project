import { Route, Routes } from "react-router-dom";
import Login from "../components/auth/Login";
import Register from "../components/auth/Register";
import CreateTask from "../components/tasks/CreateTask";
import GetMyTaskList from "../components/tasks/GetMyTaskList";
import GetTaskList from "../components/tasks/GetTaskList";
import UpdateTask from "../components/tasks/UpdateTask";

export default function IndexRoute() {
  return (
    <Routes>
      <Route path="/" element={<GetMyTaskList />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Register />} />
      <Route path="/all" element={<GetTaskList />} />
      <Route path="/create" element={<CreateTask />} />
      <Route path="/update/:id" element={<UpdateTask />} />
    </Routes>
  );
}
