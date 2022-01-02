import React, { useEffect, useState } from "react";
import { Container, LinearProgress, Fab } from "@material-ui/core";
import AddIcon from "@material-ui/icons/Add";
import { withRouter } from "react-router-dom";

import ThreadItem from "components/ThreadItem";
import ThreadsFilter from "components/ThreadsFilter";

import styles from "./HomePage.module.css";

export const HomePage = (props) => {
  const {
    threadsData: { data, loading, page, totalPages, totalItems },
    error,
    fetchThreads,
    history,
    user
  } = props;

  const [searchCriteria, setThreadTitleContent] = useState("");
  const [itemsPerPage, setItemsPerPage] = useState(5);
  const [currentPage, setCurrentPage] = useState(page || 1);

  // This effect will get triggered if any of the properties
  // used for filtering are changed, furthermore, also triggered on the first render
  useEffect(() => {
    fetchThreads(searchCriteria, currentPage, itemsPerPage);
  }, [searchCriteria, currentPage, itemsPerPage, fetchThreads]);

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
      <ThreadsFilter
        totalPages={totalPages}
        searchCriteria={searchCriteria}
        setThreadTitleContent={setThreadTitleContent}
        currentPage={currentPage}
        setCurrentPage={setCurrentPage}
        itemsPerPage={itemsPerPage}
        setItemsPerPage={setItemsPerPage}
      />
      Items found: {totalItems}
      {data?.map((t) => (
        <ThreadItem key={t.id} thread={t} onClick={history.push} />
      ))}
      {user && (
        <div className={styles.addButton}>
          <Fab
            color="primary"
            aria-label="add"
            onClick={() => history.push("/createThread")}
          >
            <AddIcon />
          </Fab>
        </div>
      )}
    </Container>
  );
};
export default withRouter(HomePage);
