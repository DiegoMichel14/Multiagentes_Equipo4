import agentpy as ap
import random
import socket
import matplotlib.pyplot as plt
from matplotlib.animation import FuncAnimation


#--Agente Vehicle--
class Vehicle(ap.Agent):
    next_number = 1  
    posx = 1


    def setup(self):
        
        self.number = Vehicle.next_number  
        self.posx = Vehicle.posx
        Vehicle.next_number += 1  
        Vehicle.posx -= 5
        self.firtPosition = - self.number
        self.movement_history = []  
        self.direction = random.randint(1, 2)
        self.rand = random.randint(1, 3)

        # Establecer la posición inicial dependiendo de la dirección
        if self.direction == 1:
            self.position = (10, self.firtPosition)
        else:
            self.position = (self.firtPosition, 10)




    #--See function--
    def is_agent_ahead_stopped(self):
    # Calcular la posición de parada según la dirección del vehículo
        if self.direction == 1:
            stop_position = (self.position[0], self.position[1] + self.rand)
        else:
            stop_position = (self.position[0] + self.rand, self.position[1])

    # Verificar si hay un vehículo detenido en la posición de parada
        for agent in self.model.agents:
            if (abs(agent.position[0] - stop_position[0]) <= 0.1) and (abs(agent.position[1] - stop_position[1]) <= 0.1) and agent.stop:
                return True
        return False


 #-Action function--
    def action(self):
        if self.model.traffic_light.state1 == "red" and self.position[0] == self.model.traffic_light.position2[0] -2 and self.direction == 2:
            self.movement_history.append(0)
            self.stop = True
        elif self.model.traffic_light.state2 == "red" and self.position[1] == self.model.traffic_light.position2[1] -2 and self.direction == 1:
            self.movement_history.append(0)
            self.stop = True
        elif self.is_agent_ahead_stopped():
            self.movement_history.append(0)
            self.stop = True
        else:
            if self.direction == 1:
                self.position = (self.position[0], self.position[1] + 1)
                self.movement_history.append(1)
                self.stop = False
            else:
                self.position = (self.position[0] + 1, self.position[1])
                self.movement_history.append(1)
                self.stop = False

       
 #--TrafficLight Agente--
class TrafficLight(ap.Agent):

    def setup(self):
        self.position1 = (10, 10)  # Posición del primer semáforo
        self.position2 = (10,10)  # Posición del segundo semáforo
        self.state1 = "green"  # Estado inicial del primer semáforo
        self.state2 = "red"    # Estado inicial del segundo semáforo
        self.steps_since_change = 0  # Contador de pasos desde el último cambio de estado

    def step(self):
        # Incrementar el contador de pasos
        self.steps_since_change += 1

        # Verificar si han pasado suficientes pasos para cambiar el estado
        if self.steps_since_change >= 10:
            # Cambiar el estado de los semáforos
            if self.state1 == "red":
                self.state1 = "green"
                self.state2 = "red"
            else:
                self.state1 = "red"
                self.state2 = "green"
            self.steps_since_change = 0

 
# --environment--
class IntersectionModel(ap.Model):
    def setup(self):
        self.agents = ap.AgentList(self, 100, Vehicle)  
        self.traffic_light = TrafficLight(self)  

    def step(self):
        for vehicle in self.agents:
            vehicle.action()
        self.traffic_light.step() 

 
model = IntersectionModel()
model.run(400)

# Obtener historial de movimiento de todos los vehículos


for agent in model.agents:
    vehicle_history = agent.movement_history
    movement_string = " ".join(map(str, vehicle_history))
    print("direccion: ", agent.direction, "posicion inicial:", agent.firtPosition, "vehículo", agent.number, ":", movement_string)

agent_info = ""
for agent in model.agents:
    agent_info += f"{agent.direction},{agent.firtPosition},{''.join(map(str, agent.movement_history))}|"



 #Imprimir la información de todos los agentes sin caracteres
print(agent_info)


'''
# Crear la figura y el eje
fig, ax = plt.subplots()

# Función de inicialización de la animación
def init():
    ax.set_xlim(-10, 30)
    ax.set_ylim(-10, 30)

# Función de actualización de la animación
def update(frame):
    model.step()
    ax.clear()
    ax.set_xlim(-10, 30)
    ax.set_ylim(-10, 30)
    for agent in model.agents:
        x, y = agent.position
        ax.plot(x, y, 'ko')  # Dibujar un punto negro en la posición del agente
        ax.text(x, y, f'{agent.number}', color='black', ha='center', va='center')  # Agregar etiqueta con el número del agente
    x1, y1 = model.traffic_light.position1
    x2, y2 = model.traffic_light.position2
    ax.plot(x1, y1, 'ro')  # Dibujar un punto rojo en la posición del primer semáforo
    ax.plot(x2, y2, 'ro')  # Dibujar un punto rojo en la posición del segundo semáforo

# Configurar la animación
ani = FuncAnimation(fig, update, frames=1000, init_func=init, interval=50)  # 200 frames, 1000 milisegundos (1 segundo) de intervalo entre frames

# Mostrar la animación
plt.show()

'''

# Establecer conexión con el servidor
s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect(("127.0.0.1", 1103))

# Recibir datos del servidor
from_server = s.recv(4096)
print("Received from server:", from_server.decode("ascii"))

# Convertir el historial de movimiento a una cadena de texto
movement_str = " ".join(map(str, model.agents[0].movement_history))

# Enviar la cadena de texto codificada en bytes
s.send(agent_info.encode("ascii"))

# Cerrar la conexión
s.close()