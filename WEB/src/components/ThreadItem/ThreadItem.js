import React from "react";
import { Card } from "@material-ui/core";

import styles from "./ThreadItem.module.css";

const ThreadItem = ({ thread, onClick }) => (
  <Card
    className={styles.container}
    raised
    onClick={() => onClick(`/threadDetails/${thread.id}`)}
  >
    <div className={styles.headerContainer}>
      <h2>{thread.title}</h2>
      <h2>{thread.author}</h2>
    </div>
    <div className={styles.content}>{thread.content}</div>
    <div className={styles.footerContainer}>
      <h3>Comments {thread.commentsCount}</h3>
    </div>
  </Card>
);

export default ThreadItem;
