import React, { Component } from 'react';
import NavMenu from "components/navMenu/navMenu";

export class Layout extends Component {
    render () {
        return (
            <div style={{display: 'flex'}}>
                <NavMenu />
                <div className={'app__content'}>
                    {this.props.children}
                </div>
            </div>
        );
    }
}
