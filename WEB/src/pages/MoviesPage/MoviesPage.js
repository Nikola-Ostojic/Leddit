import React, { useEffect, useState } from "react";
import { Container, LinearProgress } from "@material-ui/core";
import { withRouter } from "react-router-dom";

import MovieItem from "components/MovieItem";
import MoviesFilter from "components/MoviesFilter";
import CreateMovie from "components/CreateMovie";

export const MoviesPage = ({
  moviesData: { data, loading, page, totalPages, totalItems },
  error,
  fetchMovies,
  history,
  userRole
}) => {
  const [movieName, setMovieName] = useState("");
  const [itemsPerPage, setItemsPerPage] = useState(5);
  const [currentPage, setCurrentPage] = useState(page || 1);

  // This effect will get triggered if any of the properties
  // used for filtering are changed, furthermore, also triggered on the first render
  useEffect(() => {
    fetchMovies(movieName, currentPage, itemsPerPage);
  }, [movieName, currentPage, itemsPerPage, fetchMovies]);

  // When changing items per page, we always want to start from the first page
  useEffect(() => {
    setCurrentPage(1);
  }, [itemsPerPage]);

  if (error) {
    return <p>{error}</p>;
  }

  if (!data || loading) {
    return (
      <Container>
        <LinearProgress />
      </Container>
    );
  }

  return (
    <Container>
      <MoviesFilter
        totalPages={totalPages}
        movieName={movieName}
        setMovieName={setMovieName}
        currentPage={currentPage}
        setCurrentPage={setCurrentPage}
        itemsPerPage={itemsPerPage}
        setItemsPerPage={setItemsPerPage}
      />
      Items found: {totalItems}
      {data?.map((m) => (
        <MovieItem key={m.id} movie={m} onClick={history.push} />
      ))}
      {userRole === "Admin" && (
        <CreateMovie
          loadMovies={() => fetchMovies(movieName, currentPage, itemsPerPage)}
        />
      )}
    </Container>
  );
};
export default withRouter(MoviesPage);
