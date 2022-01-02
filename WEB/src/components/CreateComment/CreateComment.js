import React, { useState } from "react";
import { Button, TextField } from "@material-ui/core";
import { toast } from "react-toastify";

import { apiCall } from "utils/ApiCall";

const CreateComment = ({ threadId, onThreadCreated }) => {
  const [content, setContent] = useState("");

  const createComment = (e) => {
    e.preventDefault();
    apiCall({
      url: "/comments",
      method: "POST",
      data: { content, threadId: parseInt(threadId) },
      requiresAuth: true
    })
      .then((response) => {
        toast.success(`Comment: ${content} created!`);
        // Cleaning up the state after a successful creation
        setContent("");
        onThreadCreated();
      })
      .catch((e) => {
        toast.error("Failed to create comment");
      });
  };

  return (
    <form onSubmit={createComment}>
      <TextField
        fullWidth
        margin="normal"
        label="Comment content"
        type="text"
        required
        onChange={({ target: { value } }) => setContent(value)}
        value={content}
      />
      <Button variant="contained" color="primary" type="submit">
        Create comment
      </Button>
    </form>
  );
};

export default CreateComment;
