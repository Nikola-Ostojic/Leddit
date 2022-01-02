import React from "react";
import { Card } from "@material-ui/core";

import styles from "./CommentItem.module.css";

const CommentItem = ({ comment }) => (
  <Card className={styles.container}>
    <div className={styles.headerContainer}>
      <p>{comment.content}</p>
      <p>{comment.author}</p>
    </div>
  </Card>
);

export default CommentItem;
