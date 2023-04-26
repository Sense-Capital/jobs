# API Documentation

## **Introduction**

This API provides various methods for playing tic-tac-toe. The rules of this game can be found [here](https://en.wikipedia.org/wiki/Tic-tac-toe).


## **Methods**

### **All Games**

Returns a list of games.

#### **Request**

```http
GET /api/AllGames
```

#### **Response**

Returns a JSON response in the following format:

```javascript
{
  "row1" : string,
  "row2" : string,
  "row3" : string,
  "status" : string,
  "player1" : {
    "name" : string,
    "id" : int
  },
  "player2" : {
    "name" : string,
    "id" : int
  },
  "id" : int
}
```

The `row1`, `row2`, `row3` attributes contain an information about the playing board.

The `status` attribute describes the game status.

The `player1`, `player2` attributes contain an information about players.

The `id` attribute contains an information about game id. With this information you can find this game.

**Example:**

```javascript
[
  {
    "row1": "...",
    "row2": "...",
    "row3": "...",
    "status": "Game in process. First player's turn.",
    "turn": "X",
    "player1": {
      "name": "Vanya",
      "id": 1
    },
    "player2": {
      "name": "Masha",
      "id": 2
    },
    "id": 1
  },
  {
    "row1": ".X.",
    "row2": "...",
    "row3": "...",
    "status": "Game in process. Second player's turn.",
    "turn": "O",
    "player1": {
      "name": "Katya",
      "id": 3
    },
    "player2": {
      "name": "Masha",
      "id": 2
    },
    "id": 2
  }
]
```
### **Get Game**

Returns a game.

#### **Request**

```http
GET /api/GetGame/{id}
```

**Parameters**

| Parameter | Type | Description |
| :--- | :--- | :--- |
| `id` | `int` | **Required**. The id of the game |

#### **Response**

Returns a JSON response in the following format:

```javascript
{
  "row1" : string,
  "row2" : string,
  "row3" : string,
  "status" : string,
  "player1" : {
    "name" : string,
    "id" : int
  },
  "player2" : {
    "name" : string,
    "id" : int
  },
  "id" : int
}
```

The `row1`, `row2`, `row3` attributes contain an information about the playing board.

The `status` attribute describes the game status.

The `player1`, `player2` attributes contain an information about players.

The `id` attribute contains an information about game id. With this information you can find this game.

**Example:**
```javascript
{
    "row1": "...",
    "row2": "...",
    "row3": "...",
    "status": "Game in process. First player's turn.",
    "turn": "X",
    "player1": {
      "name": "Vanya",
      "id": 1
    },
    "player2": {
      "name": "Masha",
      "id": 2
    },
    "id": 1
  }
```
### **New Player**

Creates a new player.

#### **Request**

```http
Post /api/NewPlayer/{name}
```

**Parameters**

| Parameter | Type | Description |
| :--- | :--- | :--- |
| `name` | `string` | **Required**. Player name. |

#### **Response**

Returns a JSON response in the following format:

```javascript
{
  "name" : string,
  "id" : int
}
```

The `name` attribute contains an information about the player name.

The `id` attribute contains an information about player id. With this information you can find this player.

**Example:**
```javascript
{
  "name": "Vanya",
  "id": 1
}
```

### **New Game**

Creates a new game.

#### **Request**

```http
Post /api/NewGame
```

**Parameters**

| Parameter | Type | Description |
| :--- | :--- | :--- |
| `name1` | `string` | **Required**. First player name. |
| `name2` | `string` | **Required**. Second player name. |

#### **Response**

Returns a JSON response in the following format:

```javascript
{
  "row1" : string,
  "row2" : string,
  "row3" : string,
  "status" : string,
  "player1" : {
    "name" : string,
    "id" : int
  },
  "player2" : {
    "name" : string,
    "id" : int
  },
  "id" : int
}
```

The `row1`, `row2`, `row3` attributes contain an information about the playing board.

The `status` attribute describes the game status.

The `player1`, `player2` attributes contain an information about players.

The `id` attribute contains an information about game id. With this information you can find this game.

**Example:**
```javascript
{
    "row1": "...",
    "row2": "...",
    "row3": "...",
    "status": "Game in process. First player's turn.",
    "turn": "X",
    "player1": {
      "name": "Vanya",
      "id": 1
    },
    "player2": {
      "name": "Masha",
      "id": 2
    },
    "id": 1
  }
```
If you specify incorrect data, then the game is not created. In this case, method returns the error message:

```javascritp
"Invalid parameters."
```
### **Get Player**

Gets a player.

#### **Request**

```http
Get /api/Player/{id}
```

**Parameters**

| Parameter | Type | Description |
| :--- | :--- | :--- |
| `id` | `int` | **Required**. Player id. |

#### **Response**

Returns a JSON response in the following format:

```javascript
{
  "name" : string,
  "id" : int
}
```

The `name` attribute contains an information about the player name.

The `id` attribute contains an information about player id. With this information you can find this player.

**Example:**
```javascript
{
  "name": "Vanya",
  "id": 1
}
```

### **Delete Player**

Deletes a player.

#### **Request**

```http
Delete /api/DeletePlayer/{id}
```

**Parameters**

| Parameter | Type | Description |
| :--- | :--- | :--- |
| `id` | `int` | **Required**. Player id. |

#### **Response**

Returns a JSON response in the following format:

```javascript
{
  "message" : string
}
```

The `message` attribute contains an information about operation.

**Example:**
```javascript
{
  "Player has deleted."
}
```

### **Delete Game**

Deletes a game.

#### **Request**

```http
Delete /api/DeleteGame/{id}
```

**Parameters**

| Parameter | Type | Description |
| :--- | :--- | :--- |
| `id` | `int` | **Required**. Game id. |

#### **Response**

Returns a JSON response in the following format:

```javascript
{
  "message" : string
}
```

The `message` attribute contains an information about operation.

**Example:**
```javascript
{
  "The game has deleted."
}
```

### **Do Move**

Does a move to a game.

#### **Request**

```http
Post /api/DoMove
```

**Parameters**

| Parameter | Type | Description |
| :--- | :--- | :--- |
| `playerName` | `string` | **Required**. Player name. |
| `moveType` | `string` | **Required**. X or O. |
| `gameId` | `int` | **Required**. Game id. |
| `column` | `int` | **Required**. Column number in game board. |
| `row` | `int` | **Required**. Row number in game board. |

#### **Response**

Returns a JSON response in the following format:

```javascript
{
  "row1" : string,
  "row2" : string,
  "row3" : string,
  "status" : string,
  "player1" : {
    "name" : string,
    "id" : int
  },
  "player2" : {
    "name" : string,
    "id" : int
  },
  "id" : int
}
```

The `row1`, `row2`, `row3` attributes contain an information about the playing board.

The `status` attribute describes the game status.

The `player1`, `player2` attributes contain an information about players.

The `id` attribute contains an information about game id. With this information you can find this game.

**Example:**
```javascript
{
    "row1": "...",
    "row2": ".X.",
    "row3": "...",
    "status": "Game in process. Second player's turn.",
    "turn": "O",
    "player1": {
      "name": "Vanya",
      "id": 1
    },
    "player2": {
      "name": "Masha",
      "id": 2
    },
    "id": 1
  }
```
If you specify incorrect data, then the game is not created. In this case, method returns the error message:

```javascritp
"Invalid parameters."
```