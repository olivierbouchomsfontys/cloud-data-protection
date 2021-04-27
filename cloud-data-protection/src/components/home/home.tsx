import React from 'react';
import {Card, CardContent, Typography} from "@material-ui/core";
import './home.css';
import {Accessibility, Crop, Lock} from "@material-ui/icons";

const Home = () => {
    return (
        <div className='Home'>
            <Typography variant='h1'>Cloud Data Protection</Typography>
            <Typography variant='subtitle2'>SaaS backup solution for Google Drive</Typography>
            <p>
                Cloud Data Protection is a SaaS backup and recovery solution for Google Drive.
            </p>
            <Typography variant='h2'>Our key values</Typography>
            <div className='home__features'>
                <Card>
                    <CardContent>
                        <div className='card__title'>
                            <Crop className='card__title__icon' />
                            <h4 className='card__title__text'>
                                Flexibility
                            </h4>
                        </div>
                        <p className='card__content'>
                            Easily customize your plan to fit the needs of your organisation. No matter what happens, Cloud Data Protection will scale with you.
                        </p>
                    </CardContent>
                </Card>
                <Card>
                    <CardContent>
                        <div className='card__title'>
                            <Lock className='card__title__icon' />
                            <h4 className='card__title__text'>
                                Security
                            </h4>
                        </div>
                        <p className='card__content'>
                            Your data is our responsibility. We handle your data using the best tools and standards, such as AES-256 encryption.
                        </p>
                    </CardContent>
                </Card>
                <Card>
                    <CardContent>
                        <div className='card__title'>
                            <Accessibility className='card__title__icon' />
                            <h4 className='card__title__text'>
                                Accessibility
                            </h4>
                        </div>
                        <p className='card__content'>
                            Every company should be able to secure their data. No technical knowledge is required to use Cloud Data Protection.
                        </p>
                    </CardContent>
                </Card>
            </div>
        </div>
    );
}

export default Home;