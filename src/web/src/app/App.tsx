import React from 'react';
import './App.css';
import { Client } from '../client/api.client';

class TestComponent extends React.Component<any, any> {
  constructor(props: any) {
    super(props);
    this.state = {
      name: "None"
    };
  }

  componentDidMount() {
    let client = new Client('http://localhost:5564');
    client.getAllMemoryBookUniverses().then(models => {
      this.setState({
        name: models[0].name,
      })
    })
  }

  render() {
    return (
      <div>
        {this.state.name}
      </div>
    )
  }
}

function App() {
  return (
    <TestComponent>
      Test App Please Ignore!
    </TestComponent>
  );
}

export default App;
