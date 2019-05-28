# Docker - Visão Geral



Docker é uma plafatorma aberta para desenvolvimento, empacotamento e execução de aplicações. Docker permite que aplicações sejam separadas da infraestrutura do ambiente, permitindo a distribuição de software de forma rápida. Com Docker, você consegue gerenciar a infraestrutura de seu deploy do mesmo jeito que gerencia suas aplicações. Aproveitando as facilidades das metodologias do Docker de teste, deploy e empacotamento, você consegue reduzir significativamente o a janela de tempo entre escrever o código e rodá-lo em produção.

## A plataforma Docker

A plataforma permite que aplicações sejam executadas em um ambiente isolado que chamaremos de container. O isolamento e segurança desse componente permite que vários deles sejam executados em apenas um host. Containers são, por definição, leves, pois se comunicam diretamente com o Kernel do Host, sem necessidade de emulação, como uma máquina virtual. Isso significa que você consegue executar uma quantidade maior de containeres em comparação com máquinas virtuais completas.

O Docker fornece uma plataforma com ferramentas para gerenciar o ciclo de vida dos seus conteineres:

- Desenvolvimento: montar uma infraestrutura que consiga executar sua aplicação
- Teste: empacota sua aplicação e executa testes em um ambiente isolado e seguro
- Distribuição: envia o pacote pronto para execução local, na nuvem ou em ambiente altamente orquestado.

## Docker Engine

*Docker Engine* é uma aplicação cliente-servidor que é executado no host, sendo composto por:

- Um servidor que se comporta como um programa long-running chamado de `daemon`(o comando `dockerd`)

- Um REST API que especifica as interfaces que os programas podem se comunicar com o `daemon`e dizê-lo o que fazer.

- Uma interface de comando (CLI), mais conhecida como o comando `docker`

  ![Docker Engine Components Flow](https://docs.docker.com/engine/images/engine-components-flow.png)

A interface de linha de comando `docker`utiliza o REST API para controlar e interagir com o Docker `daemon` através de scripts ou de comandos diretos.

Através da interface é possível dizer para o `daemon`o que deve ser feito, seja a criação de imagens, containeres, redes ou volumes.

> **Note**: Docker is licensed under the open source Apache 2.0 license.

For more details, see [Docker Architecture](https://docs.docker.com/engine/docker-overview/#docker-architecture) below.



## Para quê posso usar o Docker?

**Para uma entrega rápida e consistente de suas aplicações**

Docker permite que os desenvolvedores construam suas aplicações e serviços tendo como base containeres e imagens padronizados. Containeres são ótimos para pipelines de integração e entrega contínuas.

Vamos ver um exemplo:

- Os desenvolvedores escrevem uma aplicação localmente e a compartilham através de containeres
- Então, utilizam Docker para enviar as aplicações para ambientes de testes para que possam passar por testes manuais e automatizados
- Quando desenvolvedores encontram bugs, eles conseguem corrigí-los em ambinete de desenvolvimento e rapidamente enviá-lo novamente para ambiente de testes.
- Quando o teste está completo, a publicação em ambiente de produção se torna trivial, bastando fazer o deploy do container contendo a aplicação.



**Desenvolvimento responsivo e escalonamento**

A base Docker em container permite um ambiente altamente portátil e escalonável. Como são montados independente de ambiente, conseguem ser replicados - dependendo da carga da aplicação - seja em ambiente de desenvolvimento, testes ou produção.

**Executando mais no mesmo hardware**

Containeres Docker são por sua natureza leves e rápidos. Isso permite entregar mais responsividade em comparação a máquinas virtuais executando no mesmo hardware.

## A Arquitetura Docker

Docker utiliza uma arquitetura baseada em cliente-servidor. O cliente comunica com o `daemon` (como visto anteriormente) que realiza o trabalho pesado, seja fazendo a build, executando ou distribuindo os containeres. Tanto o cliente como o `daemon` podem ser executados no mesmo host, dando controle local sobre o ambiente. Mas também é possível conectar o cliente a um `daemon` remoto,

Docker uses a client-server architecture. The Docker *client* talks to the Docker *daemon*, which does the heavy lifting of building, running, and distributing your Docker containers. The Docker client and daemon *can* run on the same system, or you can connect a Docker client to a remote Docker daemon. The Docker client and daemon communicate using a REST API, over UNIX sockets or a network interface.

![Docker Architecture Diagram](https://docs.docker.com/engine/images/architecture.svg)

### The Docker daemon

The Docker daemon (`dockerd`) listens for Docker API requests and manages Docker objects such as images, containers, networks, and volumes. A daemon can also communicate with other daemons to manage Docker services.

### The Docker client

The Docker client (`docker`) is the primary way that many Docker users interact with Docker. When you use commands such as `docker run`, the client sends these commands to `dockerd`, which carries them out. The `docker` command uses the Docker API. The Docker client can communicate with more than one daemon.

### Docker registries

A Docker *registry* stores Docker images. Docker Hub is a public registry that anyone can use, and Docker is configured to look for images on Docker Hub by default. You can even run your own private registry. If you use Docker Datacenter (DDC), it includes Docker Trusted Registry (DTR).

When you use the `docker pull` or `docker run` commands, the required images are pulled from your configured registry. When you use the `docker push` command, your image is pushed to your configured registry.

### Docker objects

When you use Docker, you are creating and using images, containers, networks, volumes, plugins, and other objects. This section is a brief overview of some of those objects.

#### IMAGES

An *image* is a read-only template with instructions for creating a Docker container. Often, an image is *based on* another image, with some additional customization. For example, you may build an image which is based on the `ubuntu` image, but installs the Apache web server and your application, as well as the configuration details needed to make your application run.

You might create your own images or you might only use those created by others and published in a registry. To build your own image, you create a *Dockerfile* with a simple syntax for defining the steps needed to create the image and run it. Each instruction in a Dockerfile creates a layer in the image. When you change the Dockerfile and rebuild the image, only those layers which have changed are rebuilt. This is part of what makes images so lightweight, small, and fast, when compared to other virtualization technologies.

#### CONTAINERS

A container is a runnable instance of an image. You can create, start, stop, move, or delete a container using the Docker API or CLI. You can connect a container to one or more networks, attach storage to it, or even create a new image based on its current state.

By default, a container is relatively well isolated from other containers and its host machine. You can control how isolated a container’s network, storage, or other underlying subsystems are from other containers or from the host machine.

A container is defined by its image as well as any configuration options you provide to it when you create or start it. When a container is removed, any changes to its state that are not stored in persistent storage disappear.

##### Example `docker run` command

The following command runs an `ubuntu` container, attaches interactively to your local command-line session, and runs `/bin/bash`.

```
$ docker run -i -t ubuntu /bin/bash
```

When you run this command, the following happens (assuming you are using the default registry configuration):

1. If you do not have the `ubuntu` image locally, Docker pulls it from your configured registry, as though you had run `docker pull ubuntu` manually.
2. Docker creates a new container, as though you had run a `docker container create` command manually.
3. Docker allocates a read-write filesystem to the container, as its final layer. This allows a running container to create or modify files and directories in its local filesystem.
4. Docker creates a network interface to connect the container to the default network, since you did not specify any networking options. This includes assigning an IP address to the container. By default, containers can connect to external networks using the host machine’s network connection.
5. Docker starts the container and executes `/bin/bash`. Because the container is running interactively and attached to your terminal (due to the `-i` and `-t` flags), you can provide input using your keyboard while the output is logged to your terminal.
6. When you type `exit` to terminate the `/bin/bash` command, the container stops but is not removed. You can start it again or remove it.

#### SERVICES

Services allow you to scale containers across multiple Docker daemons, which all work together as a *swarm* with multiple *managers*and *workers*. Each member of a swarm is a Docker daemon, and the daemons all communicate using the Docker API. A service allows you to define the desired state, such as the number of replicas of the service that must be available at any given time. By default, the service is load-balanced across all worker nodes. To the consumer, the Docker service appears to be a single application. Docker Engine supports swarm mode in Docker 1.12 and higher.

## The underlying technology

Docker is written in [Go](https://golang.org/) and takes advantage of several features of the Linux kernel to deliver its functionality.

### Namespaces

Docker uses a technology called `namespaces` to provide the isolated workspace called the *container*. When you run a container, Docker creates a set of *namespaces* for that container.

These namespaces provide a layer of isolation. Each aspect of a container runs in a separate namespace and its access is limited to that namespace.

Docker Engine uses namespaces such as the following on Linux:

- **The pid namespace:** Process isolation (PID: Process ID).
- **The net namespace:** Managing network interfaces (NET: Networking).
- **The ipc namespace:** Managing access to IPC resources (IPC: InterProcess Communication).
- **The mnt namespace:** Managing filesystem mount points (MNT: Mount).
- **The uts namespace:** Isolating kernel and version identifiers. (UTS: Unix Timesharing System).

### Control groups

Docker Engine on Linux also relies on another technology called *control groups* (`cgroups`). A cgroup limits an application to a specific set of resources. Control groups allow Docker Engine to share available hardware resources to containers and optionally enforce limits and constraints. For example, you can limit the memory available to a specific container.

### Union file systems

Union file systems, or UnionFS, are file systems that operate by creating layers, making them very lightweight and fast. Docker Engine uses UnionFS to provide the building blocks for containers. Docker Engine can use multiple UnionFS variants, including AUFS, btrfs, vfs, and DeviceMapper.

### Container format

Docker Engine combines the namespaces, control groups, and UnionFS into a wrapper called a container format. The default container format is `libcontainer`. In the future, Docker may support other container formats by integrating with technologies such as BSD Jails or Solaris Zones.