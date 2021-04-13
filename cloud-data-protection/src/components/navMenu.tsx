import React, {Fragment} from 'react';
import {useSelector} from "react-redux";
import {selectAuthenticated, selectUser} from "../features/userSlice";
import {
    Home,
    AccountCircle,
    Person,
    PersonAdd,
    ExitToApp
} from '@material-ui/icons';
import {
    AppBar, Divider,
    Drawer,
    ListItem,
    ListItemIcon,
    ListItemText,
    Toolbar,
} from "@material-ui/core";
import './navMenu.css';
import {Link} from "react-router-dom";

const NavMenu = () => {
    const authenticated = useSelector(selectAuthenticated);
    const user = useSelector(selectUser);

    const menuItems = (): JSX.Element[] => {
        const links = [];

        links.push({text: 'Home', route: '/', icon: <Home /> });

        return links.map(link => mapLink(link));
    }

    const authMenuItems = (): JSX.Element[] => {
        const links = [];

        if (authenticated) {
            links.push({text: 'Logout', route: '/logout', icon: <ExitToApp /> });
        } else {
            links.push({text: 'Login', route: '/login', icon: <Person /> });
            links.push({text: 'Register', route: '/register', icon: <PersonAdd /> });
        }

        return links.map(link => mapLink(link));
    }

    const mapLink = (link: any): JSX.Element =>  {
        return (
            <Link to={link.route} key={link.text}>
                <ListItem button className='nav__drawer__link'>
                    <ListItemIcon>{link.icon}</ListItemIcon>
                    <ListItemText primary={link.text}/>
                </ListItem>
            </Link>
        )
    }

    return (
        <Fragment>
            <AppBar className='nav__appbar' position='fixed'>
                <Toolbar className='nav__toolbar'>
                    <span className='nav__title'>
                        Cloud Data Protection
                    </span>
                    {authenticated &&
                        <div className='nav__user'>
                            <AccountCircle />
                            <span className='nav__user__text'>{user.email}</span>
                        </div>
                    }
                </Toolbar>
            </AppBar>
            <Drawer className='nav__drawer' variant='permanent' anchor='left'>
                {menuItems()}
                <Divider />
                {authMenuItems()}
            </Drawer>
        </Fragment>
    )
}

export default NavMenu;