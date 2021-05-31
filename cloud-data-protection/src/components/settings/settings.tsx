import {Tab, Tabs, Typography} from "@material-ui/core";
import React, {useState} from "react";
import AccountSettings from "components/settings/account/accountSettings";
import './settings.css';

const SettingsComponent = () => {
    const tabAccount: number = 0;

    const [selectedTab, setSelectedTab] = useState(tabAccount);

    const onTabChange = (e: any, newTab: number) => {
        setSelectedTab(newTab);
    }

    return (
        <div className='settings'>
            <Typography variant='h1'>Settings</Typography>
            <Tabs value={selectedTab} onChange={onTabChange} className='settings__tabs'>
                <Tab label='Account' id='account' />
            </Tabs>
            <div className='tab-content'>
                {selectedTab === tabAccount && <AccountSettings /> }
            </div>
        </div>
    )

}

export default SettingsComponent;