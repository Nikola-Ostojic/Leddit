import React from "react";
import MaterialDrawer from "@material-ui/core/Drawer";
import List from "@material-ui/core/List";
import Divider from "@material-ui/core/Divider";
import ListItem from "@material-ui/core/ListItem";
import ListItemIcon from "@material-ui/core/ListItemIcon";
import ListItemText from "@material-ui/core/ListItemText";
import HomeIcon from "@material-ui/icons/Home";
import StarIcon from "@material-ui/icons/Star";
import { makeStyles } from "@material-ui/core/styles";
import { withRouter } from "react-router-dom";

const useStyles = makeStyles((theme) => ({
  drawer: {
    width: 240,
    flexShrink: 0
  },
  drawerPaper: {
    width: 240,
    backgroundColor: theme.palette.primary.main,
    color: theme.palette.common.white
  },
  toolbar: theme.mixins.toolbar
}));

export const Drawer = ({ drawerOpen, setDrawerOpen, history: { push } }) => {
  const classes = useStyles();

  return (
    <MaterialDrawer
      className={classes.drawer}
      variant="temporary"
      classes={{
        paper: classes.drawerPaper
      }}
      open={drawerOpen}
      onClose={() => setDrawerOpen(false)}
    >
      <div className={classes.toolbar}></div>
      <div role="presentation" onClick={() => setDrawerOpen(false)}>
        <List>
          <ListItem button onClick={() => push("")}>
            <ListItemIcon>
              <HomeIcon htmlColor="white" />
            </ListItemIcon>
            <ListItemText primary="Home" />
          </ListItem>
        </List>
        <Divider />
        <List>
          <ListItem button onClick={() => push("/movies")}>
            <ListItemIcon>
              <StarIcon htmlColor="white" />
            </ListItemIcon>
            <ListItemText primary="Movies" />
          </ListItem>
        </List>
      </div>
    </MaterialDrawer>
  );
};

export default withRouter(Drawer);
