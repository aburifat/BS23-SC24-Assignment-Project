import { DELETE, GET, POST } from "./service";

export const GetMyTasksService = () => {
  return GET(`/api/tasks`);
};

export const GetAllTasksService = () => {
  return GET(`/api/tasks/all`);
};

export const CreateTasksService = (request) => {
  return POST(`/api/tasks/create`, request);
};

export const UpdateTasksService = (id, request) => {
  return POST(`/api/tasks/update/${id}`, request);
};

export const DeleteTasksService = (id) => {
  return DELETE(`/api/tasks/update/${id}`);
};
