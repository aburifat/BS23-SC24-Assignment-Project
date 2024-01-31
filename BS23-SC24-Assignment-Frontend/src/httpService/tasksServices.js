import { DELETE, GET, POST, PUT } from "./service";

export const GetMyTasksService = () => {
  return GET(`/api/tasks`);
};

export const GetAllTasksService = () => {
  return GET(`/api/tasks/all`);
};

export const GetTaskByIdService = (id) => {
  return GET(`/api/tasks/${id}`);
};

export const CreateTasksService = (request) => {
  return POST(`/api/tasks`, request);
};

export const UpdateTasksService = (id, request) => {
  return PUT(`/api/tasks/${id}`, request);
};

export const DeleteTasksService = (id) => {
  return DELETE(`/api/tasks/${id}`);
};
