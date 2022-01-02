import React, { useEffect, useState } from "react";

import { Button, Container, Grid, LinearProgress } from "@material-ui/core";
import DeleteIcon from "@material-ui/icons/Delete";
import { toast } from "react-toastify";

import { apiCall } from "utils/ApiCall";

import styles from "./MovieDetailsPage.module.css";

const MovieDetailsPage = ({ match, role, history }) => {
  const id = match?.params?.id;
  const [movie, setMovie] = useState(null);
  const [error, setError] = useState(null);

  useEffect(() => {
    apiCall({
      url: `/movies/${id}`,
      method: "GET",
      requiresAuth: true
    })
      .then((result) => {
        const { data } = result;
        setMovie(data);
      })
      .catch((e) => {
        setError(e.message);
      });
  }, [id]);

  if (error) {
    return <div>{error}</div>;
  }

  if (!movie && !error) {
    return <LinearProgress />;
  }

  const deleteMovie = () => {
    apiCall({
      url: `/movies/${id}`,
      method: "DELETE",
      requiresAuth: true
    })
      .then(() => {
        toast.success(`Movie: ${movie.name} deleted!`);
        history.replace("/movies");
      })
      .catch((e) => toast.error("Failed to delete movie"));
  };

  return (
    <Container>
      <Grid container justify="space-between">
        <h1>Title: {movie.name}</h1>
        {role === "Admin" && (
          <Button variant="outlined" onClick={deleteMovie}>
            <DeleteIcon />
          </Button>
        )}
      </Grid>
      <img
        className={styles.image}
        src={movie.imageUrl}
        alt="Failed to load"
      ></img>
    </Container>
  );
};

export default MovieDetailsPage;
