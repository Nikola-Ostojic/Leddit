import React from "react";
import { Card, CardContent } from "@material-ui/core";

import styles from "./MovieItem.module.css";

const MovieItem = ({ movie, onClick }) => (
  <Card
    className={styles.container}
    raised
    onClick={() => onClick(`/movieDetails/${movie.id}`)}
  >
    <CardContent>{movie.name}</CardContent>
  </Card>
);

export default MovieItem;
