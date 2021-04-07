import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/layout';
import { Home } from './components/home';

function App() {
  return (
      <Layout>
        <Route exact path='/' component={Home} />
      </Layout>
  );
}

export default App;
