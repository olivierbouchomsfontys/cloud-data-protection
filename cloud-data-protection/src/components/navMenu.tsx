import React, {Fragment} from 'react';
import {useSelector} from "react-redux";
import {selectAuthenticated, selectUser} from "../features/userSlice";
import {
    Home,
    AccountCircle,
    DriveEta,
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
import {UserRole} from "../services/result/loginResult";
import NavMenuItem from "./navMenu/navMenuItem";

const NavMenu = () => {
    const authenticated = useSelector(selectAuthenticated);
    const user = useSelector(selectUser);

    const menuItems = (): JSX.Element[] => {
        const links: NavMenuItem[] = [];

        links.push({text: 'Home', route: '/', icon: <Home /> });

        if (authenticated) {
            userMenuItems().forEach(link => links.push(link));
        }

        return links.map(link => mapLink(link));
    }

    const userMenuItems = (): NavMenuItem[] => {
        const links: NavMenuItem[] = [];

        switch (user.role) {
            case UserRole.Client:
                clientMenuItems().forEach(link => links.push(link));
                break;
            case UserRole.Employee:
                employeeMenuItems().forEach(link => links.push(link));
                break;
        }

        return links;
    }

    const clientMenuItems = (): NavMenuItem[] => {
        const links: NavMenuItem[] = [];

        links.push({text: 'Onboarding', route: '/onboarding', icon: <DriveEta />})

        return links;
    }

    const employeeMenuItems = (): NavMenuItem[] => {
        const links: NavMenuItem[] = [];

        return links;
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

    const mapLink = (link: NavMenuItem): JSX.Element =>  {
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