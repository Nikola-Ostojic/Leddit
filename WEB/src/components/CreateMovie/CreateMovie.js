import React, { useState } from "react";
import { Button, TextField } from "@material-ui/core";
import { toast } from "react-toastify";

import { apiCall } from "utils/ApiCall";

const CreateMovie = ({ loadMovies }) => {
  const [name, setName] = useState("");
  const [imageUrl, setImageUrl] = useState("");

  const createMovie = (e) => {
    e.preventDefault();
    apiCall({
      url: "/movies",
      method: "POST",
      data: { name, imageUrl },
      requiresAuth: true
    })
      .then(() => {
        toast.success(`Movie: ${name} created!`);
        // Cleaning up the state after a successful creation
        setName("");
        setImageUrl("");
      })
      .catch((e) => {
        toast.error("Failed to create movie");
      })
      .finally(loadMovies);
  };

  return (
    <form onSubmit={createMovie}>
      <TextField
        fullWidth
        margin="normal"
        label="Movie name"
        type="text"
        required
        onChange={({ target: { value } }) => setName(value)}
        value={name}
      />
      <TextField
        fullWidth
        margin="normal"
        label="Image url"
        type="text"
        onChange={({ target: { value } }) => setImageUrl(value)}
        value={imageUrl}
      />
      <Button variant="contained" color="primary" type="submit">
        Create movie
      </Button>
    </form>
  );
};

export default CreateMovie;
