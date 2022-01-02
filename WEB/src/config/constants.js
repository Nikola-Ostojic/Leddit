export const apiUrl =
  process.env.NODE_ENV === "development"
    ? "http://localhost:5000/api"
    : "https://leddit.azurewebsites.net/api";
