import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>Welcome</h1>
        <p>This application is here to help you calculating your capital gains according to the French Law.</p>
        <ul>
          <li>Have fun <a href="/capital-gain">here</a></li>
        </ul>
    </div>
    );
  }
}
