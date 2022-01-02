import React, { useState } from "react";
import { Button, TextField } from "@material-ui/core";
import { toast } from "react-toastify";

import { withRouter } from "react-router-dom";

import { apiCall } from "utils/ApiCall";

const CreateThread = ({ history }) => {
  const [title, setTitle] = useState("");
  const [content, setContent] = useState("");

  const createThread = (e) => {
    e.preventDefault();
    apiCall({
      url: "/threads",
      method: "POST",
      data: { title, content },
      requiresAuth: true
    })
      .then(() => {
        toast.success(`Thread: ${title} created!`);
        // Cleaning up the state after a successful creation
        setTitle("");
        setContent("");
      })
      .catch((e) => {
        toast.error("Failed to create thread");
      })
      .finally(() => history.goBack());
  };

  return (
    <form onSubmit={createThread}>
      <TextField
        fullWidth
        margin="normal"
        label="Thread name"
        type="text"
        required
        onChange={({ target: { value } }) => setTitle(value)}
        value={title}
      />
      <TextField
        fullWidth
        required
        margin="normal"
        label="Content"
        type="text"
        onChange={({ target: { value } }) => setContent(value)}
        value={content}
      />
      <Button variant="contained" color="primary" type="submit">
        Create thread
      </Button>
    </form>
  );
};

export default withRouter(CreateThread);
