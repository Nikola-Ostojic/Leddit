import React, { useEffect, useState } from "react";
import { Container, LinearProgress, Card } from "@material-ui/core";
import { withRouter } from "react-router-dom";

import CreateComment from "components/CreateComment";
import { apiCall } from "utils/ApiCall";
import CommentItem from "components/CommentItem";
import CommentsFilter from "components/CommentsFilter";

import styles from "./ThreadDetailsPage.module.css";

const ThreadDetailsPage = (props) => {
  const { match, error, user } = props;

  const id = match?.params?.id;
  const [thread, setThread] = useState(null);
  const [errorThread, setErrorThread] = useState(null);

  const [comments, setComments] = useState([]);
  const [errorComment, setErrorComment] = useState(null);

  const [itemsPerPage, setItemsPerPage] = useState(5);
  const [currentPage, setCurrentPage] = useState(comments.page || 1);

  useEffect(() => {
    apiCall({
      url: `/threads/${id}`,
      method: "GET"
    })
      .then((result) => {
        const { data } = result;
        setThread(data);
      })
      .catch((e) => {
        setErrorThread(e.message);
      });

    fetchComments();
  }, [id, currentPage, itemsPerPage]);

  const fetchComments = () => {
    apiCall({
      url: `comments?threadId=${id}&page=${currentPage}&itemsPerPage=${itemsPerPage}`,
      method: "GET"
    })
      .then((result) => {
        const { data } = result;
        setComments(data);
      })
      .catch((e) => {
        setErrorComment(e.message);
      });
  };

  useEffect(() => {
    setCurrentPage(1);
  }, [itemsPerPage]);

  if (error) {
    return <p>{error}</p>;
  }

  if (errorThread) {
    return <div>{errorThread}</div>;
  }

  if (!thread && !errorThread) {
    return <LinearProgress />;
  }

  if (errorComment) {
    return <div>{errorComment}</div>;
  }
  if (!comments && !errorComment) {
    return <LinearProgress />;
  }

  return (
    <div>
      <Container>
        <Card className={styles.container}>
          <div className={styles.headerContainer}>
            <h2>{thread.title}</h2>
            <h2>{thread.author}</h2>
          </div>
          <div className={styles.content}>{thread.content}</div>
        </Card>
        <br />
        <CommentsFilter
          commentsCount={thread.commentsCount}
          totalPages={comments.totalPages}
          currentPage={currentPage}
          setCurrentPage={setCurrentPage}
          itemsPerPage={itemsPerPage}
          setItemsPerPage={setItemsPerPage}
        />
        {comments.data?.map((c) => (
          <CommentItem key={c.id} comment={c} />
        ))}
        <br />
        <br />
        {user && (
          <CreateComment threadId={id} onThreadCreated={fetchComments} />
        )}
      </Container>
    </div>
  );
};

export default withRouter(ThreadDetailsPage);
