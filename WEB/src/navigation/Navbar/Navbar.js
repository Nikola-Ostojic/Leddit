import React from "react";
import { NavLink, withRouter } from "react-router-dom";
import { AppBar, Toolbar, Button, Typography } from "@material-ui/core";
import IconButton from "@material-ui/core/IconButton";
import MenuIcon from "@material-ui/icons/Menu";
import { makeStyles } from "@material-ui/core/styles";

import styles from "./Navbar.module.css";

const useStyles = makeStyles({
  appBar: {
    // Making the AppBar be "above" Drawer
    zIndex: 1400
  },
  title: {
    flexGrow: 1,
    textAlign: "left"
  }
});
export const Navbar = ({ user, logout, setDrawerOpen, drawerOpen }) => {
  const classes = useStyles();
  return (
    <AppBar position="fixed" className={`${classes.appBar} ${styles.links}`}>
      <Toolbar>
        <IconButton
          edge="start"
          color="inherit"
          aria-label="menu"
          onClick={() => setDrawerOpen(!drawerOpen)}
        >
          <MenuIcon />
        </IconButton>
        <Typography variant="h6" className={classes.title}>
          Leddit
        </Typography>
        <Button color="inherit">
          {user ? (
            <NavLink to="/auth" onClick={logout}>
              <Typography>Logout {user}</Typography>
            </NavLink>
          ) : (
            <NavLink to="/auth">
              <Typography>Login</Typography>
            </NavLink>
          )}
        </Button>
      </Toolbar>
    </AppBar>
  );
};

export default withRouter(Navbar);
